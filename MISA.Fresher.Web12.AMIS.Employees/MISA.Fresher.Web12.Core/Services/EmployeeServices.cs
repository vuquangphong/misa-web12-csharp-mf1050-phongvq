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
        #region Dependency Injection

        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeServices(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Main Method

        public object GetEmployeesPaging(int? pageIndex, int? pageSize, string? employeeFilter)
        {
            return _employeeRepository.GetEmployeesPaging(pageIndex, pageSize, employeeFilter);
        }

        #endregion
    }
}