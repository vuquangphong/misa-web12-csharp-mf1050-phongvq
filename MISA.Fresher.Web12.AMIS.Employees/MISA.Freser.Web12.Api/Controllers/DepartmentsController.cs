using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;

namespace MISA.Fresher.Web12.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        // Dependency Injection
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentServices _departmentServices;

        // Dependency Injection
        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentServices departmentServices)
        {
            _departmentRepository = departmentRepository;
            _departmentServices = departmentServices;
        }

        /// <summary>
        /// @method: GET /Departments
        /// @desc: Get the Info of all Departments
        /// @author: Vũ Quang Phong (24/01/2022)
        /// </summary>
        /// <returns>
        /// An array of Departments
        /// </returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var departments = _departmentRepository.GetAll();

                if (departments == null)
                {
                    return NoContent();
                }
                return Ok(departments);
            }
            catch (Exception ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = "Đã có lỗi xảy ra, vui lòng liên hệ với MISA!",
                };
                return StatusCode(500, res);
            }

        }
    }
}
