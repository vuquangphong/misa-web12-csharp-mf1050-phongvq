using MISA.Fresher.Web12.Core.Entities;
using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Services
{
    public class PositionServices : BaseServices<PositionE>, IPositionServices
    {
        // Dependency Injection
        private readonly IPositionRepository _positionRepository;

        // Dependency Injection
        public PositionServices(IPositionRepository positionRepository) : base(positionRepository)
        {
            _positionRepository = positionRepository;
        }
    }
}
