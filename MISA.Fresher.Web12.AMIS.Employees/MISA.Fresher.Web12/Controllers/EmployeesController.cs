using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;
using MISA.Fresher.Web12.Models;

namespace MISA.Fresher.Web12.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
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
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@EmployeeId", employeeId);

            var employee = sqlConnection.QueryFirstOrDefault<Employee>(sqlQuery, param: dynamicParameters);

            return employee;
        }
    }
}
