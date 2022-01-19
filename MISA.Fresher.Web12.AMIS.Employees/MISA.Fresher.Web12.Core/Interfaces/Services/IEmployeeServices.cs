using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Web12.Core.Entities;

namespace MISA.Fresher.Web12.Core.Interfaces.Services
{
    public interface IEmployeeServices
    {
        /// <summary>
        /// @author: Vũ Quang Phong (18/01/2022)
        /// @desc: The Service for Adding a new Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>
        /// Number of rows that are affected
        /// </returns>
        int InsertService(Employee employee);

        /// <summary>
        /// @author: Vũ Quang Phong (18/01/2022)
        /// @desc: The Service for Updating an Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeId"></param>
        /// <returns>
        /// Number of rows that are affected
        /// </returns>
        int UpdateService(Employee employee, string employeeId);
    }
}
