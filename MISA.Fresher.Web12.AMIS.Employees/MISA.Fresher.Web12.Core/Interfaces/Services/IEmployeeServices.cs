using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.OtherModels;

namespace MISA.Fresher.Web12.Core.Interfaces.Services
{
    public interface IEmployeeServices : IBaseServices<Employee>
    {
        /// <summary>
        /// @author: VQPhong (13/02/2022)
        /// @modified: VQPhong (24/06/2022)
        /// @desc: Service of Get Paging
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="employeeFilter"></param>
        /// <returns>
        /// A model of ControllerResponseData
        /// </returns>
        public ControllerResponseData GetEmployeesPaging(int? pageIndex, int? pageSize, string? employeeFilter);
    }
}
