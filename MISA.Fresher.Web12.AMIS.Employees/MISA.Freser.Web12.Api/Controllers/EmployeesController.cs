using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Web12.Api.Controllers;
using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;

namespace MISA.Fresher.Web12.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : MISABaseController<Employee>
    {
        #region Dependency Injection

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeServices _employeeServices;

        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeServices employeeServices) : base(employeeRepository, employeeServices)
        {
            _employeeRepository = employeeRepository;
            _employeeServices = employeeServices;
        }

        #endregion

        #region Main Controllers

        /// <summary>
        /// @method: GET /Employees/filter?employeeFilter=...
        /// @desc: Search for Employees by employeeFilter (code, name, phonenumber)
        /// @author: Vũ Quang Phong (20/01/2022)
        /// </summary>
        /// <param name="employeeFilter"></param>
        /// <returns>
        /// An object contains the Number of Records & the Array of Employees
        /// </returns>
        [HttpGet("filter")]
        public IActionResult GetByFilter(string? employeeFilter)
        {
            try
            {
                var employees = _employeeRepository.GetByFilter(employeeFilter);

                if (!employees.Any())
                {
                    return NoContent();
                }

                var res = new
                {
                    TotalRecord = employees.Count(),
                    Data = employees
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return CatchException(ex);
            }
        }

        /// <summary>
        /// @method: GET /Employees/paging?pageIndex=...&pageSize=...
        /// @desc: Paging Employees data
        /// @author: Vũ Quang Phong (28/01/2022)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// 
        /// </returns>
        [HttpGet("paging")]
        public IActionResult GetPaging(int pageIndex, int pageSize)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return CatchException(ex);
            }
            
        }

        #endregion Main Controllers
    }
}
