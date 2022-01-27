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
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeServices _employeeServices;

        // Dependency Injection
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeServices employeeServices)
        {
            _employeeRepository = employeeRepository;
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
                var employees = _employeeRepository.GetAll();

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
                var employee = _employeeRepository.GetById(employeeId);

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
        public IActionResult Put(Employee employee, Guid employeeId)
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
                var rowsEffect = _employeeRepository.DeleteById(employeeId);

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
