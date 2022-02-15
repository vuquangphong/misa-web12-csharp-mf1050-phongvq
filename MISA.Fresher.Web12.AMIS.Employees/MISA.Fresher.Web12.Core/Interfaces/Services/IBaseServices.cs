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
        public int InsertService(T entity);

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
        public int UpdateService(T entity, Guid entityId);

        /// <summary>
        /// @author: Vũ Quang Phong (14/02/2022)
        /// @desc: The Service of Removing multiple Entities by an array Ids
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns>
        /// Number of Rows affected
        /// </returns>
        public int DeleteMultiService(string[] entityIds);
    }
}
