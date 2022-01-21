using Dapper;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        #region Some properties

        private const string _server = "47.241.69.179";
        private const string _port = "3306";
        private const string _database = "MISA.CukCuk_Demo_NVMANH_copy";
        private const string _user_id = "dev";
        private const string _password = "manhmisa";

        private readonly string _entityName = typeof(T).Name;

        protected MySqlConnection? SqlConnection;
        protected DynamicParameters? DynamicParams;

        #endregion

        #region Support Methods
        private static string GetConnectionString()
        {
            return "" +
                $"Server = '{_server}'; " +
                $"Port = '{_port}'; " +
                $"Database = '{_database}'; " +
                $"User Id = '{_user_id}'; " +
                $"Password = '{_password}'";
        }

        protected static MySqlConnection ConnectDatabase()
        {
            // Declare the info of Database
            string connectionString = GetConnectionString();

            // Initital Connection
            var sqlConnection = new MySqlConnection(connectionString);

            return sqlConnection;
        }

        #endregion

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Getting all the Entities <T> from Database
        /// </summary>
        /// <returns>
        /// An array of Entities <T>
        /// </returns>
        public IEnumerable<T> GetAll()
        {
            using (SqlConnection = ConnectDatabase())
            {
                var entities = SqlConnection.Query<T>($"SELECT * FROM {_entityName}");
                return entities;
            }
        }

        /// <summary>
        /// @author:Vũ Quang Phong (21/01/2022)
        /// @desc: Getting an Entity <T> from Database by Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// An Entity
        /// </returns>
        public T GetById(string entityId)
        {
            using (SqlConnection = ConnectDatabase())
            {
                // Create dynamic parameters
                DynamicParams = new DynamicParameters();
                DynamicParams.Add($"@{_entityName}Id", entityId);

                // Query data in database
                var sqlQuery = $"SELECT * FROM {_entityName} WHERE {_entityName}Id = @{_entityName}Id";
                var entity = SqlConnection.QueryFirstOrDefault<T>(sqlQuery, param: DynamicParams);

                return entity;
            }
        }

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Search for Entities by Filter (code, name, phonenumber)
        /// </summary>
        /// <param name="entitiesFilter"></param>
        /// <returns>
        /// An array of Entities
        /// </returns>
        public IEnumerable<T> GetByFilter(string entitiesFilter)
        {
            using (SqlConnection = ConnectDatabase())
            {
                // Create dynamic parameters
                DynamicParams = new DynamicParameters();
                DynamicParams.Add($"@{_entityName}Filter", $"%{entitiesFilter}%");

                // Query data in database
                var sqlQuery = $"SELECT * FROM {_entityName} WHERE " +
                    $"{_entityName}Code LIKE @{_entityName}Filter " +
                    $"OR FullName LIKE @{_entityName}Filter " +
                    $"OR PhoneNumber LIKE @{_entityName}Filter";
                var entities = SqlConnection.Query<T>(sqlQuery, param: DynamicParams);

                return entities;
            }
        }

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Inserting a new record into Entity Database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int Insert(T entity)
        {
            using (SqlConnection = ConnectDatabase())
            {
                // Create dynamic parameters
                DynamicParams = new DynamicParameters();

                var properties = entity.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(entity);
                    var propertyType = property.PropertyType;

                    if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    {
                        DynamicParams.Add($"@{propertyName}", propertyValue, DbType.String);
                    }
                    else
                    {
                        DynamicParams.Add($"@{propertyName}", propertyValue);
                    }
                }

                // Query data in database
                var sqlQuery = "" +
                    "INSERT INTO " +
                    $"{_entityName}({_entityName}Id, {_entityName}Code, FirstName, LastName, FullName, Gender, " +
                        "PhoneNumber, Email, Address, DateOfBirth)" +
                    $"VALUES(@{_entityName}Id, @{_entityName}Code, @FirstName, @LastName, @FullName, @Gender, " +
                        "@PhoneNumber, @Email, @Address, @DateOfBirth)";

                var rowsEffect = SqlConnection.Execute(sqlQuery, param: DynamicParams);

                return rowsEffect;
            }
        }

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Updating an Entity by Id
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int UpdateById(T entity, string entityId)
        {
            using (SqlConnection = ConnectDatabase())
            {
                // Create dynamic parameters
                DynamicParams = new DynamicParameters();

                var properties = entity.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(entity);
                    var propertyType = property.PropertyType;

                    if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                    {
                        DynamicParams.Add($"@{propertyName}", propertyValue, DbType.String);
                    }
                    else
                    {
                        DynamicParams.Add($"@{propertyName}", propertyValue);
                    }
                }

                // Query data in database
                var sqlQuery = "" +
                    $"UPDATE {_entityName} " +
                    $"SET {_entityName}Code = @{_entityName}Code, FirstName = @FirstName, LastName = @LastName, " +
                        "FullName = @FullName, Gender = @Gender, PhoneNumber = @PhoneNumber, " +
                        "Email = @Email, Address = @Address, DateOfBirth = @DateOfBirth " +
                    $"WHERE EmployeeId = @EmployeeId";

                var rowsEffect = SqlConnection.Execute(sqlQuery, param: DynamicParams);

                return rowsEffect;
            }
        }

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Check if the current EntityCode is duplicate
        /// </summary>
        /// <param name="entityCode"></param>
        /// <returns>
        /// True <--> EntityCode Coincidence
        /// False <--> No EentityCode Coincidence
        /// </returns>
        public bool IsDuplicateCode(string entityCode)
        {
            using (SqlConnection = ConnectDatabase())
            {
                // Create dynamic parameters
                DynamicParams = new DynamicParameters();
                DynamicParams.Add($"@{_entityName}Code", entityCode);

                // Query data in database
                var sqlCheck = $"SELECT {_entityName}Code FROM {_entityName} WHERE {_entityName}Code = @{_entityName}Code";
                var isExist = SqlConnection.QueryFirstOrDefault(sqlCheck, param: DynamicParams);

                if (isExist == null)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Removing an Entity from Database
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int DeleteById(string entityId)
        {
            using (SqlConnection = ConnectDatabase())
            {
                // Create dynamic parameters
                DynamicParams = new DynamicParameters();
                DynamicParams.Add($"@{_entityName}Id", entityId);

                // Query data in Database
                var sqlQuery = $"DELETE FROM {_entityName} WHERE {_entityName}Id = @{_entityName}Id";
                var rowEffects = SqlConnection.Execute(sqlQuery, param: DynamicParams);

                return rowEffects;
            }
        }
    }
}
