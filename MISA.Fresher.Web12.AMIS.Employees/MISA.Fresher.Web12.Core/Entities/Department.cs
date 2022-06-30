using MISA.Fresher.Web12.Core.MISAAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Entities
{
    public class Department
    {
        // Primary Key
        [PrimaryKey]
        public Guid? DepartmentId { get; set; }

        // Department Code
        [NotEmpty]
        [NotDuplicated]
        [PropsName("Mã đơn vị")]
        public string? DepartmentCode { get; set; }

        // Department Name
        [NotEmpty]
        [NotDuplicated]
        [PropsName("Tên đơn vị")]
        public string? DepartmentName { get; set; }

        // Description of the Department
        public string? Description { get; set; }
    }
}
