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
    public class EmployeeServices : BaseServices<Employee>, IEmployeeServices
    {
        // Dependency Injection
        private readonly IEmployeeRepository _employeeRepository;

        // Dependency Injection
        public EmployeeServices(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
    }
}