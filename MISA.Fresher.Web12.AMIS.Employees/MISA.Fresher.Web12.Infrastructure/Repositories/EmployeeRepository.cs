﻿using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MISA.Fresher.Web12.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public object GetEmployeesPaging(int? pageIndex, int? pageSize, string? employeeFilter)
        {
            DynamicParams = new DynamicParameters();
            DynamicParams.Add("@m_PageIndex", pageIndex);
            DynamicParams.Add("@m_PageSize", pageSize);
            DynamicParams.Add("@m_EmployeeFilter", employeeFilter);
            DynamicParams.Add("@m_TotalRecords", direction: ParameterDirection.Output, dbType: DbType.Int32);
            DynamicParams.Add("@m_TotalPages", direction: ParameterDirection.Output, dbType: DbType.Int32);

            using (SqlConnection = ConnectDatabase())
            {
                var employeesPaging = SqlConnection.Query<Employee>(
                    "Proc_GetEmployeePaging", 
                    param: DynamicParams, 
                    commandType: CommandType.StoredProcedure
                );

                int totalRecords = DynamicParams.Get<int>("@m_TotalRecords");
                int totalPages = DynamicParams.Get<int>("@m_TotalPages");

                return new
                {
                    TotalRecords = totalRecords, 
                    TotalPages = totalPages, 
                    Data = employeesPaging,
                };
            }
        }
    }
}
