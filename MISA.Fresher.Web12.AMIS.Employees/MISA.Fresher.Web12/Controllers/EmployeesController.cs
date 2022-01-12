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
    public class EmployeesController : ControllerBase
    {
        #region Support Methods
        /// <summary>
        /// @desc: Get the Info of Database Connection
        /// @author: Vũ Quang Phong (11/01/2022)
        /// </summary>
        private string getConnectionString()
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
        public IEnumerable<Employee> Get()
        {
            try
            {
                // Declare the info of Database
                string connectionString = getConnectionString();

                // Initital Connection
                var sqlConnection = new MySqlConnection(connectionString);

                // Query data in database
                var employees = sqlConnection.Query<Employee>("SELECT * FROM Employee");

                return employees;
            }
            catch (Exception ex)
            {
                return (IEnumerable<Employee>)StatusCode(500, ex.Message);
            }
            
        }

        /// <summary>
        /// @method: GET /Employees/{employeeId}
        /// @desc: Ge tthe Info of an Employee by Id
        /// @author: Vũ Quang Phong (11/01/2022)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>
        /// The Employee corresponding
        /// </returns>
        [HttpGet("{employeeId}")]
        public Employee Get(string employeeId)
        {
            // Declare the info of Database
            string connectionString = getConnectionString();

            // Initital Connection
            var sqlConnection = new MySqlConnection(connectionString);

            // Query data in database
            var sqlQuery = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@EmployeeId", employeeId);

            var employee = sqlConnection.QueryFirstOrDefault<Employee>(sqlQuery, param: dynamicParams);

            return employee;
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
            // Create EmployeeId
            _employee.EmployeeId = new Guid();

            // Validate data from request
            var properties = _employee.GetType().GetProperties();
            var dynamicParams = new DynamicParameters();

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

            // Declare the info of Database
            string connectionString = getConnectionString();

            // Initital Connection
            var sqlConnection = new MySqlConnection(connectionString);

            // Query data in database
            var sqlQuery = "" +
                "INSERT INTO " +
                "Employee(EmployeeId, EmployeeCode, FirstName, LastName, FullName, Gender, " +
                    "PhoneNumber, Email, Address, DateOfBirth)" +
                "VALUES(@EmployeeId, @EmployeeCode, @FirstName, @LastName, @FullName, @Gender, " +
                    "@PhoneNumber, @Email, @Address, @DateOfBirth)";

            var insertAction = sqlConnection.Query<Employee>(sqlQuery, param: dynamicParams);

            return Created("201", insertAction);

            //var rowEffects = sqlConnection.Execute("Proc_InsertCustomer", dynamicParams, commandType: CommandType.Text);

            //if (rowEffects > 0)
            //{
            //    return Created("201", _employee);
            //}
            //else
            //{
            //    return NoContent();
            //}

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
            // Validate data from request
            var properties = _employee.GetType().GetProperties();
            var dynamicParams = new DynamicParameters();

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

            // Declare the info of Database
            string connectionString = getConnectionString();

            // Initital Connection
            var sqlConnection = new MySqlConnection(connectionString);

            // Query data in database
            var sqlQuery = "" +
                "UPDATE Employee " +
                "SET EmployeeCode = @EmployeeCode, FirstName = @FirstName, LastName = @LastName, " +
                    "FullName = @FullName, Gender = @Gender, PhoneNumber = @PhoneNumber, " +
                    "Email = @Email, Address = @Address, DateOfBirth = @DateOfBirth " +
                "WHERE EmployeeId = @EmployeeId";

            var updateAction = sqlConnection.QueryFirstOrDefault<Employee>(sqlQuery, param: dynamicParams);

            return Created("201", updateAction);
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
        public string Delete(string employeeId)
        {
            // Declare the info of Database
            string connectionString = getConnectionString();

            // Initital Connection
            var sqlConnection = new MySqlConnection(connectionString);

            // Query data in Database
            var sqlQuery = "DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@EmployeeId", employeeId);

            var deleteAction = sqlConnection.QueryFirstOrDefault<Employee>(sqlQuery, param: dynamicParams);

            return "Delete hah?";
        }

        #endregion
    }
}
