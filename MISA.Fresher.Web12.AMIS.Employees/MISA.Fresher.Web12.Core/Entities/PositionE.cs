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
        public Guid? PositionEId { get; set; }

        // Position Code
        public string? PositionECode { get; set; }

        // Position Name
        public string? PositionEName { get; set; }

        // Description of the Position
        public string? Description { get; set; }
    }
}
