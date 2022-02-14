using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Web12.Core.Entities;

namespace MISA.Fresher.Web12.Core.Interfaces.Infrastructure
{
    /// <summary>
    /// @author: Vũ Quang Phong (21/01/2022)
    /// @desc: Implementation --> See EmployeeRepos in Infrastructure project
    /// </summary>
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// @author: Vũ Quang Phong (13/02/2022)
        /// @desc: Get all Employees (join with Department and Position)
        /// </summary>
        /// <returns>
        /// A list of Employees
        /// </returns>
        public List<Employee> GetAllEmployees();

        /// <summary>
        /// @author: Vũ Quang Phong (04/02/2022)
        /// @desc: Get a list of Employees by PageIndex and PageSize, and/or searchText
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchText"></param>
        /// <returns>
        /// A list of Employees
        /// </returns>
        public object GetEmployeesPaging(int? pageIndex, int? pageSize, string? employeeFilter);
    }
}
