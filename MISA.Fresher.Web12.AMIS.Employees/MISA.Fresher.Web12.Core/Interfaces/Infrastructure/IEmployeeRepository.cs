﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Web12.Core.Entities;

namespace MISA.Fresher.Web12.Core.Interfaces.Infrastructure
{
    /// <summary>
    /// @author: Vũ Quang Phong (21/01/2022)
    /// @desc: Implementation --> See EmployeeRepos in Infrastructure project
    /// </summary>
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        
    }
}
