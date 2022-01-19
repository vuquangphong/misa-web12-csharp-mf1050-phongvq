using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;

namespace MISA.Fresher.Web12.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        // Dependency Injection
        private readonly IEmployeeRepositories _employeeRepositories;
        private readonly IEmployeeServices _employeeServices;

        // Dependency Injection
        public EmployeesController(IEmployeeRepositories employeeRepositories, IEmployeeServices employeeServices)
        {
            _employeeRepositories = employeeRepositories;
            _employeeServices = employeeServices;
        }

        /// <summary>
        /// @method: GET /Employees
        /// @desc: Get the Info of all Employees
        /// @author: Vũ Quang Phong (19/01/2022)
        /// </summary>
        /// <returns>
        /// An array of Employees
        /// </returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var employees = _employeeRepositories.GetAll();

                if (employees == null)
                {
                    return NoContent();
                }
                return Ok(employees);
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

        /// <summary>
        /// @method: GET /Employees/{employeeId}
        /// @desc: Get the Info of an Employee by Id
        /// @author: Vũ Quang Phong (19/01/2022)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>
        /// The Employee corresponding
        /// </returns>
        [HttpGet("{employeeId}")]
        public IActionResult Get(string employeeId)
        {
            try
            {
                var employee = _employeeRepositories.GetById(employeeId);

                if (employee == null)
                {
                    return NoContent();
                }
                return Ok(employee);
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

        /// <summary>
        /// @method: POST /Employees
        /// @desc: Insert a new Employee into Database
        /// @author: Vũ Quang Phong (19/01/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>
        /// A Message (Success or Fail)
        /// </returns>
        [HttpPost]
        public IActionResult Post(Employee employee)
        {
            try
            {
                var rowsEffect = _employeeServices.InsertService(employee);

                if (rowsEffect > 0)
                {
                    return StatusCode(201, employee);
                }
                return StatusCode(204);
            }
            catch (MISAValidateException ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                };
                return BadRequest(res);
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

        /// <summary>
        /// @method: PUT /Employees/{employeeId}
        /// @desc: Update some Info of an Employee
        /// @author: Vũ Quang Phong (19/01/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeId"></param>
        /// <returns>
        /// A Message (Success or Fail)
        /// </returns>
        [HttpPut("{employeeId}")]
        public IActionResult Put(Employee employee ,string employeeId)
        {
            try
            {
                var rowsEffect = _employeeServices.UpdateService(employee, employeeId);

                if (rowsEffect > 0)
                {
                    return StatusCode(201, rowsEffect);
                }
                return StatusCode(204);
            }
            catch (MISAValidateException ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Message,
                };
                return BadRequest(res);
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

        /// <summary>
        /// @method: DELETE /Employees/{employeeId}
        /// @desc: Remove an Employee by Id
        /// @author: Vũ Quang Phong (19/01/2022)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>
        /// A Message (Success or Fail)
        /// </returns>
        [HttpDelete("{employeeId}")]
        public IActionResult Delete(string employeeId)
        {
            try
            {
                var rowsEffect = _employeeRepositories.DeleteById(employeeId);

                if (rowsEffect > 0)
                {
                    return Ok(rowsEffect);
                }
                return NoContent();
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
