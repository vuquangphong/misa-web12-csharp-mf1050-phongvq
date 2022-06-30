using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;

namespace MISA.Fresher.Web12.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MISABaseController<T> : ControllerBase
    {
        #region Dependency Injection

        private readonly IBaseRepository<T> _baseRepository;
        private readonly IBaseServices<T> _baseServices;

        public MISABaseController(IBaseRepository<T> baseRepository, IBaseServices<T> baseServices)
        {
            _baseRepository = baseRepository;
            _baseServices = baseServices;
        }

        #endregion

        #region Main Controllers

        /// <summary>
        /// @method: GET /Entities
        /// @desc: Get the Info of all Entities
        /// @author: VQPhong (28/01/2022)
        /// @modified: VQPhong (20/06/2022)
        /// </summary>
        /// <returns>
        /// An array of Entities
        /// </returns>
        [HttpGet]
        public IActionResult Get()
        {
            var res = _baseServices.GetAllData();
            return Ok(res);
        }

        /// <summary>
        /// @method: GET /Entities/{entityId}
        /// @desc: Get the Info of an Entity by Id
        /// @author: VQPhong (28/01/2022)
        /// @modified: VQPhong (24/06/2022)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// The Entity corresponding
        /// </returns>
        [HttpGet("{entityId}")]
        public IActionResult Get(string entityId)
        {
            var res = _baseServices.GetDataById(entityId);
            return Ok(res);
        }

        /// <summary>
        /// @method: POST /Entities
        /// @desc: Insert a new Entity into Database
        /// @author: VQPhong (28/01/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// A Message (Success or Fail)
        /// </returns>
        [HttpPost]
        public IActionResult Post(T entity)
        {
            var res = _baseServices.InsertData(entity);
            return Ok(res);
        }

        /// <summary>
        /// @method: PUT /Entities/{entityId}
        /// @desc: Update some Info of an Entity
        /// @author: Vũ Quang Phong (28/01/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns>
        /// A Message (Success or Fail)
        /// </returns>
        [HttpPut("{entityId}")]
        public IActionResult Put(T entity, Guid entityId)
        {
            var res = _baseServices.UpdateData(entity, entityId);
            return Ok(res);
        }

        /// <summary>
        /// @method: DELETE /Entities/{entityId}
        /// @desc: Remove an Entity by Id
        /// @author: Vũ Quang Phong (28/01/2022)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// A Message (Success or Fail)
        /// </returns>
        [HttpDelete("{entityId}")]
        public IActionResult Delete(string entityId)
        {
            var res = _baseServices.DeleteData(entityId);
            return Ok(res);
        }

        /// <summary>
        /// @method: PATCH(DEL) /Entities/{entityIds}
        /// @desc: Remove multiple Entities by an array of Ids
        /// @author: Vũ Quang Phong (14/02/2022)
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns>
        /// A Message
        /// </returns>
        [HttpPatch]
        public IActionResult DeleteMulti(string[] entityIds)
        {

            var res = _baseServices.DeleteMultiData(entityIds);
            return Ok(res);
        }

        #endregion
    }
}
