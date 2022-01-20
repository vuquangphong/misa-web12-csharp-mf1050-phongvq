using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MISA.Fresher.Web12.Infrastructure.Repositories
{
    public class EmployeeRepositories : IEmployeeRepositories
    {
        #region Constants of Info of Database
        private const string SERVER = "47.241.69.179";
        private const string PORT = "3306";
        private const string DATABASE = "MISA.CukCuk_Demo_NVMANH_copy";
        private const string USER_ID = "dev";
        private const string PASSWORD = "manhmisa";
        #endregion

        #region Support Methods
        private static string GetConnectionString()
        {
            return "" +
                $"Server = '{SERVER}'; " +
                $"Port = '{PORT}'; " +
                $"Database = '{DATABASE}'; " +
                $"User Id = '{USER_ID}'; " +
                $"Password = '{PASSWORD}'";
        }

        private static MySqlConnection ConnectDatabase()
        {
            // Declare the info of Database
            string connectionString = GetConnectionString();

            // Initital Connection
            var sqlConnection = new MySqlConnection(connectionString);

            return sqlConnection;
        }

        #endregion

        #region Main Methods

        public IEnumerable<Employee> GetAll()
        {
            // Connect to Database
            var sqlConnection = ConnectDatabase();

            // Query data in database
            var employees = sqlConnection.Query<Employee>("SELECT * FROM Employee");

            return employees;
        }

        public Employee GetById(string employeeId)
        {
            // Connect to database
            var sqlConnection = ConnectDatabase();

            // Create dynamic parameters
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@EmployeeId", employeeId);

            // Query data in database
            var sqlQuery = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
            var employee = sqlConnection.QueryFirstOrDefault<Employee>(sqlQuery, param: dynamicParams);

            return employee;
        }

        public IEnumerable<Employee> GetByFilter(string employeeFilter)
        {
            // Connect to database
            var sqlConnection = ConnectDatabase();

            // Create dynamic parameters
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@EmployeeFilter", $"%{employeeFilter}%");

            // Query data in database
            var sqlQuery = "SELECT * FROM Employee WHERE " +
                "EmployeeCode LIKE @EmployeeFilter " +
                "OR FullName LIKE @EmployeeFilter " +
                "OR PhoneNumber LIKE @EmployeeFilter";
            var employees = sqlConnection.Query<Employee>(sqlQuery, param: dynamicParams);

            return employees;
        }

        public int Insert(Employee employee)
        {
            // Connect to database
            var sqlConnection = ConnectDatabase();

            // Create dynamic parameters
            var dynamicParams = new DynamicParameters();

            var properties = employee.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(employee);
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

            var rowsEffect = sqlConnection.Execute(sqlQuery, param: dynamicParams);

            return rowsEffect;
        }

        public int UpdateById(Employee employee, string employeeId)
        {
            // Connect to database
            var sqlConnection = ConnectDatabase();

            // Create dynamic parameters
            var dynamicParams = new DynamicParameters();

            var properties = employee.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(employee);
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

            var rowsEffect = sqlConnection.Execute(sqlQuery, param: dynamicParams);

            return rowsEffect;
        }

        public bool IsDuplicateEmployeeCode(string employeeCode)
        {
            // Connect to database
            var sqlConnection = ConnectDatabase();

            // Create dynamic parameters
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@EmployeeCode", employeeCode);

            // Query data in database
            var sqlCheck = "SELECT EmployeeCode FROM Employee WHERE EmployeeCode = @EmployeeCode";
            var isExist = sqlConnection.QueryFirstOrDefault(sqlCheck, param: dynamicParams);

            if (isExist == null)
            {
                return false;
            }
            return true;
        }

        public int DeleteById(string employeeId)
        {
            // Connect to database
            var sqlConnection = ConnectDatabase();

            // Create dynamic parameters
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@EmployeeId", employeeId);

            // Query data in Database
            var sqlQuery = "DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
            var rowEffects = sqlConnection.Execute(sqlQuery, param: dynamicParams);

            return rowEffects;
        }

        #endregion
    }
}
