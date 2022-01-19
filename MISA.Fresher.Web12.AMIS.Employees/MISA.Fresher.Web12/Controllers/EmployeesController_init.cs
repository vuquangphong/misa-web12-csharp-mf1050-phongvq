using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using MISA.Fresher.Web12.Models;
using System.Data;

namespace MISA.Fresher.Web12.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController_init : ControllerBase
    {
        #region Support Methods
        /// <summary>
        /// @desc: Get the Info of Database Connection
        /// @author: Vũ Quang Phong (11/01/2022)
        /// </summary>
        private static string GetConnectionString()
        {
            DotNetEnv.Env.Load();
            var _server = Environment.GetEnvironmentVariable("SERVER");
            var _port = Environment.GetEnvironmentVariable("PORT");
            var _database = Environment.GetEnvironmentVariable("DATABASE");
            var _user_id = Environment.GetEnvironmentVariable("USER_ID");
            var _password = Environment.GetEnvironmentVariable("PASSWORD");

            return "" +
                $"Server = '{_server}'; " +
                $"Port = '{_port}'; " +
                $"Database = '{_database}'; " +
                $"User Id = '{_user_id}'; " +
                $"Password = '{_password}'";
        }
        #endregion

        #region Controllers

        /// <summary>
        /// @method: GET /Employees
        /// @desc: Get the Info of all Employees
        /// @author: Vũ Quang Phong (11/01/2022)
        /// </summary>
        /// <returns>
        /// An array of Employees
        /// </returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // Declare the info of Database
                string connectionString = GetConnectionString();

                // Initital Connection
                var sqlConnection = new MySqlConnection(connectionString);

                // Query data in database
                var employees = sqlConnection.Query<Employee>("SELECT * FROM Employee");

                // Response
                if (employees == null)
                {
                    //var res = new
                    //{
                    //    devMsg = "No data",
                    //    userMsg = "Dữ liệu chưa có nhân viên nào!",
                    //};
                    return StatusCode(204);
                    //return NoContent(); ~ It's OKAY!
                }
                return StatusCode(200, employees);
                //return Ok(employees); ~ It's OKAY!

            }
            catch (Exception ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = "Đã có lỗi xảy ra, vui lòng liên hệ với MISA!",
                };
                return StatusCode(500, res);
            }
            
        }

        /// <summary>
        /// @method: GET /Employees/{employeeId}
        /// @desc: Get the Info of an Employee by Id
        /// @author: Vũ Quang Phong (11/01/2022)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>
        /// The Employee corresponding
        /// </returns>
        [HttpGet("{employeeId}")]
        public IActionResult Get(string employeeId)
        {
            try
            {
                // Declare the info of Database
                string connectionString = GetConnectionString();

                // Initital Connection
                var sqlConnection = new MySqlConnection(connectionString);
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add("@EmployeeId", employeeId);

                // Query data in database
                var sqlQuery = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
                var employee = sqlConnection.QueryFirstOrDefault<Employee>(sqlQuery, param: dynamicParams);

                // Response
                if (employee == null)
                {
                    return StatusCode(204);
                }
                return StatusCode(200, employee);

            }
            catch (Exception ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = "Đã có lỗi xảy ra, vui lòng liên hệ với MISA!",
                };
                return StatusCode(500, res);
            }
        }

        /// <summary>
        /// @method: POST /Employees
        /// @desc: Insert a new Employee into Database
        /// @author: Vũ Quang Phong (12/01/2022)
        /// </summary>
        /// <param name="_employee"></param>
        /// <returns>
        /// A Message (Success or Fail)
        /// </returns>
        [HttpPost]
        public IActionResult Post(Employee _employee)
        {
            try
            {
                // Declare the info of Database
                string connectionString = GetConnectionString();

                // Initital Connection
                var sqlConnection = new MySqlConnection(connectionString);
                var dynamicParams = new DynamicParameters();

                // Create EmployeeId
                _employee.EmployeeId = Guid.NewGuid();

                // Validate data from request
                // 1. Handling empty EmployeeCode
                if (string.IsNullOrEmpty(_employee.EmployeeCode))
                {
                    var res = new
                    {
                        devMsg = "Empty EmployeeCode!",
                        userMsg = "Mã nhân viên không được phép để trống!",
                    };
                    return StatusCode(400, res);
                    //return BadRequest(res); => It's OKAY!
                }

                // 2. Handling duplicate EmployeeCode
                dynamicParams.Add("@EmployeeCode", _employee.EmployeeCode);
                var sqlCheck = "SELECT EmployeeCode FROM Employee WHERE EmployeeCode = @EmployeeCode";
                var isExist = sqlConnection.QueryFirstOrDefault(sqlCheck, param: dynamicParams);

                if (isExist != null)
                {
                    var res = new
                    {
                        devMsg = "Duplicate EmployeeCode!",
                        userMsg = "Mã nhân viên này đã tồn tại, vui lòng nhập lại!",
                    };
                    return StatusCode(400, res);
                }

                // 3. Handling empty FullName
                if (string.IsNullOrEmpty(_employee.FullName))
                {
                    var res = new
                    {
                        devMsg = "Empty FullName!",
                        userMsg = "Tên nhân viên không được phép để trống!",
                    };
                    return StatusCode(400, res);
                }

                // Everything OK
                // Create Dynamic Params
                var properties = _employee.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(_employee);
                    var propertyType = property.PropertyType;

                    if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    {
                        dynamicParams.Add($"@{propertyName}", propertyValue, DbType.String);
                    }
                    else
                    {
                        dynamicParams.Add($"@{propertyName}", propertyValue);
                    }
                }

                // Query data in database
                var sqlQuery = "" +
                    "INSERT INTO " +
                    "Employee(EmployeeId, EmployeeCode, FirstName, LastName, FullName, Gender, " +
                        "PhoneNumber, Email, Address, DateOfBirth)" +
                    "VALUES(@EmployeeId, @EmployeeCode, @FirstName, @LastName, @FullName, @Gender, " +
                        "@PhoneNumber, @Email, @Address, @DateOfBirth)";

                var rowEffects = sqlConnection.Execute(sqlQuery, param: dynamicParams);

                // Response
                if (rowEffects > 0)
                {
                    return StatusCode(201, rowEffects);
                    //return Created(rowEffects); ~ It's OKAY!
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = "Đã có lỗi xảy ra, vui lòng liên hệ với MISA!",
                };
                return StatusCode(500, res);
            }
        }

        /// <summary>
        /// @method: PUT /Employees/{employeeId}
        /// @desc: Update some Info of an Employee
        /// @author: Vũ Quang Phong (12/01/2022)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="_employee"></param>
        /// <returns>
        /// A Message
        /// </returns>
        [HttpPut("{employeeId}")]
        public IActionResult Put(string employeeId, Employee _employee)
        {
            try
            {
                // Declare the info of Database
                string connectionString = GetConnectionString();

                // Initital Connection
                var sqlConnection = new MySqlConnection(connectionString);
                var dynamicParams = new DynamicParameters();

                // Validate data from request
                // 1. Handling empty EmployeeCode
                if (string.IsNullOrEmpty(_employee.EmployeeCode))
                {
                    var res = new
                    {
                        devMsg = "Empty EmployeeCode!",
                        userMsg = "Mã nhân viên không được phép để trống!",
                    };
                    return StatusCode(400, res);
                    //return BadRequest(res); => It's OKAY!
                }

                // 2. Handling duplicate EmployeeCode
                dynamicParams.Add("@EmployeeCode", _employee.EmployeeCode);
                var sqlCheck = "SELECT EmployeeCode FROM Employee WHERE EmployeeCode = @EmployeeCode";
                var isExist = sqlConnection.QueryFirstOrDefault(sqlCheck, param: dynamicParams);

                if (isExist == null)
                {
                    var res = new
                    {
                        devMsg = "Duplicate EmployeeCode!",
                        userMsg = "Mã nhân viên này đã tồn tại, vui lòng nhập lại!",
                    };
                    return StatusCode(400, res);
                }

                // 3. Handling empty FullName
                if (string.IsNullOrEmpty(_employee.FullName))
                {
                    var res = new
                    {
                        devMsg = "Empty FullName!",
                        userMsg = "Tên nhân viên không được phép để trống!",
                    };
                    return StatusCode(400, res);
                }

                // Everything OK
                // Create Dynamic Params
                var properties = _employee.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(_employee);
                    var propertyType = property.PropertyType;

                    if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    {
                        dynamicParams.Add($"@{propertyName}", propertyValue, DbType.String);
                    }
                    else
                    {
                        dynamicParams.Add($"@{propertyName}", propertyValue);
                    }
                }

                // Query data in database
                var sqlQuery = "" +
                    "UPDATE Employee " +
                    "SET EmployeeCode = @EmployeeCode, FirstName = @FirstName, LastName = @LastName, " +
                        "FullName = @FullName, Gender = @Gender, PhoneNumber = @PhoneNumber, " +
                        "Email = @Email, Address = @Address, DateOfBirth = @DateOfBirth " +
                    "WHERE EmployeeId = @EmployeeId";

                var updateAction = sqlConnection.QueryFirstOrDefault<Employee>(sqlQuery, param: dynamicParams);

                return StatusCode(200, updateAction);

            }
            catch (Exception ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = "Đã có lỗi xảy ra, vui lòng liên hệ với MISA!",
                };
                return StatusCode(500, res);
            }
        }

        /// <summary>
        /// @method: DELETE /Employees/{employeeId}
        /// @desc: Remove an Employee by Id
        /// @author: Vũ Quang Phong (12/01/2022)
        /// </summary>
        /// <param name="_employeeId"></param>
        /// <returns>
        /// A Message
        /// </returns>
        [HttpDelete("{employeeId}")]
        public IActionResult Delete(string employeeId)
        {
            try
            {
                // Declare the info of Database
                string connectionString = GetConnectionString();

                // Initital Connection
                var sqlConnection = new MySqlConnection(connectionString);
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add("@EmployeeId", employeeId);

                // Query data in Database
                var sqlQuery = "DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
                var rowEffects = sqlConnection.Execute(sqlQuery, param: dynamicParams);

                // Response
                if (rowEffects > 0)
                {
                    return StatusCode(200, rowEffects);
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = "Đã có lỗi xảy ra, vui lòng liên hệ với MISA!",
                };
                return StatusCode(500, res);
            }
        }

        #endregion
    }
}
