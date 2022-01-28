using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;

namespace MISA.Fresher.Web12.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : MISABaseController<PositionE>
    {
        #region Dependency Injection
        
        private readonly IPositionRepository _positionRepository;
        private readonly IPositionServices _positionServices;

        public PositionsController(IPositionRepository positionRepository, IPositionServices positionServices) : base(positionRepository, positionServices)
        {
            _positionRepository = positionRepository;
            _positionServices = positionServices;
        }

        #endregion
    }
}
