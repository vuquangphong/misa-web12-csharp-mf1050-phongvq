using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;

namespace MISA.Fresher.Web12.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        // Dependency Injection
        private readonly IPositionRepository _positionRepository;
        private readonly IPositionServices _positionServices;

        // Dependency Injection
        public PositionsController(IPositionRepository positionRepository, IPositionServices positionServices)
        {
            _positionRepository = positionRepository;
            _positionServices = positionServices;
        }

        /// <summary>
        /// @method: GET /Positions
        /// @desc: Get the Info of all Positions
        /// @author: Vũ Quang Phong (24/01/2022)
        /// </summary>
        /// <returns>
        /// An array of Positions
        /// </returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var positions = _positionRepository.GetAll();

                if (positions == null)
                {
                    return NoContent();
                }
                return Ok(positions);
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
