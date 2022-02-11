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

        // Information of  Database
        private const string _server = "13.229.200.157";
        private const string _port = "3306";
        private const string _database = "WEB12.2021.MISA.PHONGVQ";
        private const string _user_id = "dev";
        private const string _password = "12345678";

        private readonly string _entityName = typeof(T).Name;

        protected MySqlConnection? SqlConnection;
        protected DynamicParameters? DynamicParams;

        #endregion

        #region Support Methods

        /// <summary>
        /// @desc: Create the connection string from Database Info
        /// @author: Vũ Quang Phong (21/01/2022)
        /// </summary>
        /// <returns>
        /// The connection string
        /// </returns>
        private static string GetConnectionString()
        {
            return "" +
                $"Server = '{_server}'; " +
                $"Port = '{_port}'; " +
                $"Database = '{_database}'; " +
                $"User Id = '{_user_id}'; " +
                $"Password = '{_password}'";
        }

        /// <summary>
        /// @desc: Create MySQL Connection from Connection string
        /// @author: Vũ Quang Phong (21/01/2022)
        /// </summary>
        /// <returns>
        /// The MySqlConnection
        /// </returns>
        protected static MySqlConnection ConnectDatabase()
        {
            // Declare the info of Database
            string connectionString = GetConnectionString();

            // Initital Connection
            var sqlConnection = new MySqlConnection(connectionString);

            return sqlConnection;
        }

        #endregion

        #region Main Methods

        public IEnumerable<T> GetAll()
        {
            using (SqlConnection = ConnectDatabase())
            {
                var entities = SqlConnection.Query<T>($"SELECT * FROM {_entityName}");
                return entities;
            }
        }

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
                    $"OR {_entityName}Name LIKE @{_entityName}Filter " +
                    $"OR PhoneNumber LIKE @{_entityName}Filter";
                var entities = SqlConnection.Query<T>(sqlQuery, param: DynamicParams);

                return entities;
            }
        }

        public int Insert(T entity)
        {
            // Sql string query
            var sqlColumnsName = new StringBuilder();
            var sqlColumnsValue = new StringBuilder();
            string delimiter = "";

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

                sqlColumnsName.Append($"{delimiter}{propertyName}");
                sqlColumnsValue.Append($"{delimiter}@{propertyName}");
                delimiter = ", ";
            }

            var sqlQuery = $"INSERT INTO {_entityName}({sqlColumnsName}) VALUES ({sqlColumnsValue})";

            using (SqlConnection = ConnectDatabase())
            {
                var rowsEffect = SqlConnection.Execute(sqlQuery, param: DynamicParams);

                return rowsEffect;
            }
        }

        public int UpdateById(T entity, Guid entityId)
        {
            // Sql string query
            var sqlSetColumns = new StringBuilder();
            string delimiter = "";

            // Create dynamic parameters
            DynamicParams = new DynamicParameters();
            DynamicParams.Add($"@{_entityName}Id", entityId, DbType.String);

            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                if (propertyName == $"{_entityName}Id")
                {
                    property.SetValue(entity, entityId);
                }
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

                sqlSetColumns.Append($"{delimiter}{propertyName} = @{propertyName}");
                delimiter = ", ";
            }

            // Query data in database
            var sqlQuery = $"UPDATE {_entityName} SET {sqlSetColumns} WHERE {_entityName}Id = @{_entityName}Id";

            using (SqlConnection = ConnectDatabase())
            {
                var rowsEffect = SqlConnection.Execute(sqlQuery, param: DynamicParams);

                return rowsEffect;
            }
        }

        public bool IsDuplicateCode(string entityCode, string entityId, bool isPut)
        {
            using (SqlConnection = ConnectDatabase())
            {
                DynamicParams = new DynamicParameters();
                DynamicParams.Add($"@{_entityName}Code", entityCode);

                if (isPut)
                {
                    DynamicParams.Add($"@{_entityName}Id", entityId);

                    var sqlQuery = $"SELECT * FROM {_entityName} WHERE {_entityName}Id = @{_entityName}Id";
                    var currentEntity = SqlConnection.QueryFirstOrDefault<T>(sqlQuery, param: DynamicParams);
                    var propsCurEntity = currentEntity.GetType().GetProperties();

                    foreach (var prop in propsCurEntity)
                    {
                        if (prop.GetValue(currentEntity) != null)
                        {
                            var propValue = prop.GetValue(currentEntity).ToString();
                            if (propValue == entityCode)
                            {
                                return false;
                            }
                        }
                    }

                }
                
                var sqlCheck = $"SELECT {_entityName}Code FROM {_entityName} WHERE {_entityName}Code = @{_entityName}Code";
                var isExist = SqlConnection.QueryFirstOrDefault(sqlCheck, param: DynamicParams);

                if (isExist == null)
                {
                    return false;
                }
                return true;         
               
            }
        }

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

        #endregion
    }
}
