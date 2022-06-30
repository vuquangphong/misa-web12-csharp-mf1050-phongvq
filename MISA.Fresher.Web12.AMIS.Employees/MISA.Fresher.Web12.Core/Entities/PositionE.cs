using MISA.Fresher.Web12.Core.MISAAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Entities
{
    public class PositionE
    {
        // Primary Key
        [PrimaryKey]
        public Guid? PositionEId { get; set; }

        // Position Code
        [NotEmpty]
        [NotDuplicated]
        [PropsName("Mã chức vụ")]
        public string? PositionECode { get; set; }

        // Position Name
        [NotEmpty]
        [NotDuplicated]
        [PropsName("Tên chức vụ")]
        public string? PositionEName { get; set; }

        // Description of the Position
        public string? Description { get; set; }
    }
}
