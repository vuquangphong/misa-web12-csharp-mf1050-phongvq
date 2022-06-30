using MISA.Fresher.Web12.Core.OtherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Interfaces.Services
{
    public interface IBaseServices<T>
    {
        /// <summary>
        /// @author: VQPhong (14/02/2022)
        /// @modified: VQPhong (20/06/2022)
        /// @desc: The Service of GetAll
        /// </summary>
        /// <returns>
        /// A model of ControllerResponseData
        /// </returns>
        public ControllerResponseData GetAllData();

        /// <summary>
        /// @author: VQPhong (14/02/2022)
        /// @modified: VQPhong (24/06/2022)
        /// @desc: The service of Get by Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// A model of ControllerResponseData
        /// </returns>
        public ControllerResponseData GetDataById(string entityId);

        /// <summary>
        /// @author: VQPhong (24/01/2022)
        /// @desc: The Service for Adding a new Entity
        /// @modified: VQPhong (24/06/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// A model of ControllerResponseData
        /// </returns>
        public ControllerResponseData InsertData(T entity);

        /// <summary>
        /// @author: VQPhong (24/01/2022)
        /// @desc: The Service for Updating an Entity
        /// @modified: VQPhong (24/06/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns>
        /// A model of ControllerResponseData
        /// </returns>
        public ControllerResponseData UpdateData(T entity, Guid entityId);

        /// <summary>
        /// @author: VQPhong (14/02/2022)
        /// @desc: The Service of Removing an Entity by Id
        /// @modified: VQPhong (24/06/2022)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// A model of ControllerResponseData
        /// </returns>
        public ControllerResponseData DeleteData(string entityId);

        /// <summary>
        /// @author: VQPhong (14/02/2022)
        /// @modified: VQPhong (24/06/2022)
        /// @desc: The Service of Removing multiple Entities by an array Ids
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns>
        /// A model of ControllerResponseData
        /// </returns>
        public ControllerResponseData DeleteMultiData(string[] entityIds);
    }
}
