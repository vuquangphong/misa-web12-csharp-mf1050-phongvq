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
        /// @author: Vũ Quang Phong (24/01/2022)
        /// @desc: The Service for Adding a new Entity
        /// @edited: Vũ Quang Phong (26/01/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// Number of rows that are affected
        /// </returns>
        int InsertService(T entity);

        /// <summary>
        /// @author: Vũ Quang Phong (24/01/2022)
        /// @desc: The Service for Updating an Entity
        /// @edited: Vũ Quang Phong (26/01/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns>
        /// Number of rows that are affected
        /// </returns>
        int UpdateService(T entity, Guid entityId);
    }
}
