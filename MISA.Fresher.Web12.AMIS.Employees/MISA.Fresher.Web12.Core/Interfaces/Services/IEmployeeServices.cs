using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Web12.Core.Entities;

namespace MISA.Fresher.Web12.Core.Interfaces.Services
{
    public interface IEmployeeServices : IBaseServices<Employee>
    {
        /// <summary>
        /// @author: Vũ Quang Phong (13/02/2022)
        /// @desc: Service of Get Paging
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchText"></param>
        /// <returns>
        /// An Object
        /// </returns>
        public object GetEmployeesPaging(int? pageIndex, int? pageSize, string? employeeFilter);
    }
}
