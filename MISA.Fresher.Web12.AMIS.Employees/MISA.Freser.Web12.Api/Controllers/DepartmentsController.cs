using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;

namespace MISA.Fresher.Web12.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : MISABaseController<Department>
    {
        #region Dependency Injection

        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentServices _departmentServices;

        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentServices departmentServices) : base(departmentRepository, departmentServices)
        {
            _departmentRepository = departmentRepository;
            _departmentServices = departmentServices;
        }

        #endregion
    }
}
