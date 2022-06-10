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

        #region Support Method

        /// <summary>
        /// @desc: Catching Exceptions from Server Errors
        /// @author: VQPhong (28/01/2022)
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>
        /// Status Code 500
        /// </returns>
        protected IActionResult CatchException(Exception ex)
        {
            var res = new
            {
                devMsg = ex.Message,
                userMsg = Core.Resources.ResourceVietnam.UserMsgServerError,
            };
            return StatusCode(500, res);
        }

        #endregion

        #region Main Controllers

        /// <summary>
        /// @method: GET /Entities
        /// @desc: Get the Info of all Entities
        /// @author: VQPhong (28/01/2022)
        /// @modified: VQPhong (03/06/2022)
        /// </summary>
        /// <returns>
        /// An array of Entities
        /// </returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var entities = _baseServices.GetAllService();

                if (entities  == null || entities?.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(entities);
            }
            catch (Exception ex)
            {
                return CatchException(ex);
            }

        }

        /// <summary>
        /// @method: GET /Entities/{entityId}
        /// @desc: Get the Info of an Entity by Id
        /// @author: Vũ Quang Phong (28/01/2022)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// The Entity corresponding
        /// </returns>
        [HttpGet("{entityId}")]
        public IActionResult Get(string entityId)
        {
            try
            {
                var entity = _baseServices.GetByIdService(entityId);

                if (entity == null)
                {
                    return NoContent();
                }
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return CatchException(ex);
            }

        }

        /// <summary>
        /// @method: POST /Entities
        /// @desc: Insert a new Entity into Database
        /// @author: Vũ Quang Phong (28/01/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// A Message (Success or Fail)
        /// </returns>
        [HttpPost]
        public IActionResult Post(T entity)
        {
            try
            {
                var rowsEffect = _baseServices.InsertService(entity);

                if (rowsEffect > 0)
                {
                    return StatusCode(201, entity);
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
                return CatchException(ex);
            }
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
            try
            {
                var rowsEffect = _baseServices.UpdateService(entity, entityId);

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
                return CatchException(ex);
            }
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
            try
            {
                var rowsEffect = _baseServices.DeleteService(entityId);

                if (rowsEffect > 0)
                {
                    return Ok(rowsEffect);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return CatchException(ex);
            }
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
            try
            {
                var rowsEffect = _baseServices.DeleteMultiService(entityIds);

                if (rowsEffect > 0)
                {
                    return Ok(rowsEffect);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return CatchException(ex);
            }
        }

        #endregion
    }
}
