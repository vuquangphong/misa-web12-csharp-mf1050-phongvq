using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Services
{
    public class DepartmentServices : IDepartmentServices
    {
        // Dependency Injection
        private readonly IDepartmentRepository _departmentRepository;

        // Dependency Injection
        public DepartmentServices(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public int InsertService(Department department)
        {
            // Validate data from request
            // 1. Handling duplicate DepartmentCode
            if (!string.IsNullOrEmpty(department.DepartmentCode))
            {
                if (_departmentRepository.IsDuplicateCode(department.DepartmentCode))
                {
                    throw new MISAValidateException("Mã đơn vị này đã tồn tại, vui lòng nhập lại!");
                }
            }

            // 2. Handling empty DepartmentName
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                throw new MISAValidateException("Tên đơn vị không được phép để trống!");
            }

            // Everything is Okay!
            // Add a new Department to Database
            department.DepartmentId = Guid.NewGuid();
            int rowsEffect = _departmentRepository.Insert(department);

            return rowsEffect;
        }

        public int UpdateService(Department department, string departmentId)
        {
            // Validate data from request
            // 1. Handling duplicate DepartmentCode
            if (!string.IsNullOrEmpty(department.DepartmentCode))
            {
                if (_departmentRepository.IsDuplicateCode(department.DepartmentCode))
                {
                    throw new MISAValidateException("Mã đơn vị này đã tồn tại, vui lòng nhập lại!");
                }
            }

            // 2. Handling empty DepartmentName
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                throw new MISAValidateException("Tên đơn vị không được phép để trống!");
            }

            // Everything is Okay!
            // Update a Department in Database
            int rowsEffect = _departmentRepository.UpdateById(department, departmentId);

            return rowsEffect;
        }
    }
}
