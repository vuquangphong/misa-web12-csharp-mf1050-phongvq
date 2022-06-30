using Dapper;
using Microsoft.Extensions.Configuration;
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

        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly string _entityName = typeof(T).Name;

        protected MySqlConnection? SqlConnection;
        protected DynamicParameters? DynamicParams;

        /// <summary>
        /// @desc: Constructor for Passing (Injection) connection string from appsettings.json
        /// @author: VQPhong (08/06/2022)
        /// </summary>
        /// <param name="configuration"></param>
        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("VQPHONG");
        }

        #endregion

        #region Support Methods

        /// <summary>
        /// @desc: Create MySQL Connection from Connection string
        /// @author: Vũ Quang Phong (21/01/2022)
        /// </summary>
        /// <returns>
        /// The MySqlConnection
        /// </returns>
        protected MySqlConnection ConnectDatabase()
        {
            // Initital Connection
            var sqlConnection = new MySqlConnection(_connectionString);

            return sqlConnection;
        }

        #endregion

        #region Main Methods

        public IEnumerable<T> GetAll()
        {
            using (SqlConnection = ConnectDatabase())
            {
                string sqlString = $"Proc_GetAll{_entityName}";

                var entities = SqlConnection.Query<T>(
                    sqlString,
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return entities;
            }
        }

        public T GetById(string entityId)
        {
            using (SqlConnection = ConnectDatabase())
            {
                // Create dynamic parameters
                DynamicParams = new DynamicParameters();
                DynamicParams.Add($"@m_{_entityName}Id", entityId);

                // Query data in database
                var sqlQuery = $"Proc_Get{_entityName}ById";

                var entity = SqlConnection.QueryFirstOrDefault<T>(
                    sqlQuery,
                    param: DynamicParams,
                    commandType: CommandType.StoredProcedure
                );

                return entity;
            }
        }

        public int Insert(T entity)
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
                    DynamicParams.Add($"@m_{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    DynamicParams.Add($"@m_{propertyName}", propertyValue);
                }
            }

            var sqlQuery = $"Proc_Create{_entityName}";

            using (SqlConnection = ConnectDatabase())
            {
                var rowsEffect = SqlConnection.Execute(
                    sqlQuery,
                    param: DynamicParams,
                    commandType: CommandType.StoredProcedure
                );

                return rowsEffect;
            }
        }

        public int UpdateById(T entity, Guid entityId)
        {
            // Create dynamic parameters
            DynamicParams = new DynamicParameters();
            DynamicParams.Add($"@m_{_entityName}Id", entityId, DbType.String);

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
                    DynamicParams.Add($"@m_{propertyName}", propertyValue, DbType.String);
                }
                else
                {
                    DynamicParams.Add($"@m_{propertyName}", propertyValue);
                }
            }

            // Query data in database
            var sqlQuery = $"Proc_Update{_entityName}";

            using (SqlConnection = ConnectDatabase())
            {
                var rowsEffect = SqlConnection.Execute(
                    sqlQuery,
                    param: DynamicParams,
                    commandType: CommandType.StoredProcedure
                );

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

                    var sqlQuery = $"SELECT {_entityName}Code FROM {_entityName} WHERE {_entityName}Id = @{_entityName}Id";
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
                DynamicParams.Add($"@m_{_entityName}Id", entityId);

                // Query data in Database
                var sqlQuery = $"Proc_Delete{_entityName}ById";
                var rowsEffect = SqlConnection.Execute(
                    sqlQuery, 
                    param: DynamicParams, 
                    commandType: CommandType.StoredProcedure
                );

                return rowsEffect;
            }
        }

        public int DeleteMultiById(string[] entityIds)
        {
            // Init query string
            //StringBuilder idCompare = new StringBuilder();

            string delimeter = "";
            var concatIdString = new StringBuilder();

            DynamicParams = new DynamicParameters();

            //foreach (var (id, index) in entityIds.Select((id, index) => (id, index)))
            //{
            //    DynamicParams.Add($"@{_entityName}Id{index}", id, DbType.String);

            //    // Method1: WHERE Id = ... OR Id = ... OR ...
            //    //idCompare.Append($"{delimeter}{_entityName}Id = @{_entityName}Id{index}");
            //    //delimeter = " OR ";

            //    // Method2: WHERE Id IN (...)
            //    idCompare.Append($"{delimeter}@{_entityName}Id{index}");
            //    delimeter = ", ";
            //}

            // Method3: Using Stored Procedure
            foreach (string id in entityIds)
            {
                concatIdString.Append($"{delimeter}{id}");
                delimeter = ",";
            }

            DynamicParams.Add($"@m_String{_entityName}Id", concatIdString, DbType.String);
            DynamicParams.Add("@m_Delimiter", delimeter);

            var sqlQuery = $"Proc_DeleteMulti{_entityName}ById";

            using (SqlConnection = ConnectDatabase())
            {
                var rowsEffect = SqlConnection.Execute(
                    sqlQuery,
                    param: DynamicParams,
                    commandType: CommandType.StoredProcedure
                );
                return rowsEffect;
            }
        }

        #endregion
    }
}
