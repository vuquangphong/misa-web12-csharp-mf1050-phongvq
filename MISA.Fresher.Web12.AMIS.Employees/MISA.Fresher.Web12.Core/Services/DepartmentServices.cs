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
    public class DepartmentServices : BaseServices<Department>, IDepartmentServices
    {
        // Dependency Injection
        private readonly IDepartmentRepository _departmentRepository;

        // Dependency Injection
        public DepartmentServices(IDepartmentRepository departmentRepository) : base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
    }
}
