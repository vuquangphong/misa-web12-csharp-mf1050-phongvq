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
    public class PositionServices : IPositionServices
    {
        // Dependency Injection
        private readonly IPositionRepository _positionRepository;

        // Dependency Injection
        public PositionServices(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public int InsertService(PositionE position)
        {
            // Validate data from request
            // 1. Handling duplicate PositionCode
            if (!string.IsNullOrEmpty(position.PositionECode))
            {
                if (_positionRepository.IsDuplicateCode(position.PositionECode))
                {
                    throw new MISAValidateException("Mã chức vụ này đã tồn tại, vui lòng nhập lại!");
                }
            }

            // 2. Handling empty PositionName
            if (string.IsNullOrEmpty(position.PositionEName))
            {
                throw new MISAValidateException("Tên chức vụ không được phép để trống!");
            }

            // Everything is Okay!
            // Add a new Position to Database
            position.PositionEId = Guid.NewGuid();
            int rowsEffect = _positionRepository.Insert(position);

            return rowsEffect;
        }

        public int UpdateService(PositionE position, string positionId)
        {
            // Validate data from request
            // 1. Handling duplicate PositionCode
            if (!string.IsNullOrEmpty(position.PositionECode))
            {
                if (_positionRepository.IsDuplicateCode(position.PositionECode))
                {
                    throw new MISAValidateException("Mã chức vụ này đã tồn tại, vui lòng nhập lại!");
                }
            }

            // 2. Handling empty PositionName
            if (string.IsNullOrEmpty(position.PositionEName))
            {
                throw new MISAValidateException("Tên chức vụ không được phép để trống!");
            }

            // Everything is Okay!
            // Update a Department in Database
            int rowsEffect = _positionRepository.UpdateById(position, positionId);

            return rowsEffect;
        }
    }
}
