using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Web12.Core.Interfaces.Services;
using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;

namespace MISA.Fresher.Web12.Core.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        // Dependency Injection
        private readonly IEmployeeRepositories _employeeRepositories;

        // Dependency Injection
        public EmployeeServices(IEmployeeRepositories employeeRepositories)
        {
            _employeeRepositories = employeeRepositories;
        }

        public int InsertService(Employee employee)
        {
            // Validate data from request
            // 1. Handling empty EmployeeCode
            if (string.IsNullOrEmpty(employee.EmployeeCode))
            {
                throw new MISAValidateException("Mã nhân viên không được phép để trống!");
            }

            // 2. Handling empty FullName
            if (string.IsNullOrEmpty(employee.FullName))
            {
                throw new MISAValidateException("Tên nhân viên không được phép để trống!");
            }

            // 3. Handling duplicate EmployeeCode
            if (_employeeRepositories.IsDuplicateEmployeeCode(employee.EmployeeCode))
            {
                throw new MISAValidateException("Mã nhân viên này đã tồn tại, vui lòng nhập lại!");
            }

            // Everything is Okay!
            // Add a new Employee to Database
            employee.EmployeeId = Guid.NewGuid();
            int rowsEffect = _employeeRepositories.Insert(employee);

            return rowsEffect;
        }

        public int UpdateService(Employee employee, string employeeId)
        {
            // Validate data from request
            // 1. Handling empty EmployeeCode
            if (string.IsNullOrEmpty(employee.EmployeeCode))
            {
                throw new MISAValidateException("Mã nhân viên không được phép để trống!");
            }

            // 2. Handling empty FullName
            if (string.IsNullOrEmpty(employee.FullName))
            {
                throw new MISAValidateException("Tên nhân viên không được phép để trống!");
            }

            // 3. Handling duplicate EmployeeCode
            if (_employeeRepositories.IsDuplicateEmployeeCode(employee.EmployeeCode))
            {
                throw new MISAValidateException("Mã nhân viên này đã tồn tại, vui lòng nhập lại!");
            }

            // Everything is Okay!
            // Update an Employee in Database
            int rowsEffect = _employeeRepositories.UpdateById(employee, employeeId);

            return rowsEffect;
        }
    }
}
