﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Entities
{
    public class Department
    {
        // Primary Key
        public Guid? DepartmentId { get; set; }

        // Department Code
        public string? DepartmentCode { get; set; }

        // Department Name
        public string? DepartmentName { get; set; }

        // Description of the Department
        public string? Description { get; set; }
    }
}