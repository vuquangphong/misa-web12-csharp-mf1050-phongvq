using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Web12.Core.Entities;

namespace MISA.Fresher.Web12.Core.Interfaces.Infrastructure
{
    /// <summary>
    /// Implementation --> See EmployeeRepos in Infrastructure project
    /// </summary>
    public interface IEmployeeRepositories
    {
        /// <summary>
        /// @author: Vũ Quang Phong (19/01/2022)
        /// @desc: Getting all the Employeess from Database
        /// </summary>
        /// <returns>
        /// An array of Employees
        /// </returns>
        public IEnumerable<Employee> GetAll();

        /// <summary>
        /// @author:Vũ Quang Phong (19/01/2022)
        /// @desc: Getting an Employee from Database by Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>
        /// An Employee
        /// </returns>
        public Employee GetById(string employeeId);

        /// <summary>
        /// @author: Vũ Quang Phong (20/01/2022)
        /// @desc: Search for Employees by Filter (code, name, phonenumber)
        /// </summary>
        /// <param name="employeeFilter"></param>
        /// <returns>
        /// An array of Employees
        /// </returns>
        public IEnumerable<Employee> GetByFilter(string employeeFilter);

        /// <summary>
        /// @author: Vũ Quang Phong (19/01/2022)
        /// @desc: Inserting a new record into Employee Database
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int Insert(Employee employee);

        /// <summary>
        /// @author: Vũ Quang Phong (19/01/2022)
        /// @desc: Updating an Employee by Id
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeId"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int UpdateById(Employee employee, string employeeId);

        /// <summary>
        /// @author: Vũ Quang Phong (19/01/2022)
        /// @desc: Check if the current EmployeeCode is duplicate
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>
        /// True <--> EmployeeCode Coincidence
        /// False <--> Not EmployeeCode Coincidence
        /// </returns>
        public bool IsDuplicateEmployeeCode(string employeeCode);

        /// <summary>
        /// @author: Vũ Quang Phong (19/01/2022)
        /// @desc: Removing an Employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int DeleteById(string employeeId);
    }
}
