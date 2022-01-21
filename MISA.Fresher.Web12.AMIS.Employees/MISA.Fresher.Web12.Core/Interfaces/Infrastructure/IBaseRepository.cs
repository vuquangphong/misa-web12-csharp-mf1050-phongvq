using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Interfaces.Infrastructure
{
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Getting all the Entities <T> from Database
        /// </summary>
        /// <returns>
        /// An array of Entities <T>
        /// </returns>
        public IEnumerable<T> GetAll();

        /// <summary>
        /// @author:Vũ Quang Phong (21/01/2022)
        /// @desc: Getting an Entity <T> from Database by Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// An Entity
        /// </returns>
        public T GetById(string entityId);

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Search for Entities by Filter (code, name, phonenumber)
        /// </summary>
        /// <param name="entitiesFilter"></param>
        /// <returns>
        /// An array of Entities
        /// </returns>
        public IEnumerable<T> GetByFilter(string entitiesFilter);

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Inserting a new record into Entity Database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int Insert(T entity);

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Updating an Entity by Id
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int UpdateById(T entity, string entityId);

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Check if the current EntityCode is duplicate
        /// </summary>
        /// <param name="entityCode"></param>
        /// <returns>
        /// True <--> EntityCode Coincidence
        /// False <--> No EentityCode Coincidence
        /// </returns>
        public bool IsDuplicateCode(string entityCode);

        /// <summary>
        /// @author: Vũ Quang Phong (21/01/2022)
        /// @desc: Removing an Entity from Database
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        /// A number of rows which is affected
        /// </returns>
        public int DeleteById(string entityId);
    }
}
