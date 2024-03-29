﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Interfaces.Infrastructure
{
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// @author: VQPhong (21/01/2022)
        /// @modified: VQPhong (09/06/2022)
        /// @desc: Getting all the Entities <T> from Database
        /// </summary>
        /// <returns>
        /// An array of Entities <T>
        /// </returns>
        public IEnumerable<T> GetAll();

        /// <summary>
        /// @author: VQPhong (21/01/2022)
        /// @modified: VQPhong (09/06/2022)
        /// @desc: Getting an Entity <T> from Database by Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// An Entity
        /// </returns>
        public T GetById(string entityId);

        /// <summary>
        /// @author: VQPhong (21/01/2022)
        /// @desc: Inserting a new record into Entity Database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int Insert(T entity);

        /// <summary>
        /// @author: VQPhong (21/01/2022)
        /// @desc: Updating an Entity by Id
        /// @edited: VQPhong (28/01/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int UpdateById(T entity, Guid entityId);

        /// <summary>
        /// @author: VQPhong (21/01/2022)
        /// @edited: VQPhong (26/01/2022)
        /// @desc: Check if the current EntityCode is duplicate
        /// </summary>
        /// <param name="entityCode"></param>
        /// <returns>
        /// True <--> EntityCode Coincidence
        /// False <--> No EntityCode Coincidence
        /// </returns>
        public bool IsDuplicateCode(string entityCode, string entityId, bool isPut);

        /// <summary>
        /// @author: VQPhong (21/01/2022)
        /// @modified: VQPhong (09/06/2022)
        /// @desc: Removing an Entity from Database
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int DeleteById(string entityId);

        /// <summary>
        /// @author: VQPhong (14/02/2022)
        /// @modified: VQPhong (10/06/2022)
        /// @desc: Removing Multiple Entities by an array of Ids
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns>
        /// The number of rows affected
        /// </returns>
        public int DeleteMultiById(string[] entityIds);
    }
}
