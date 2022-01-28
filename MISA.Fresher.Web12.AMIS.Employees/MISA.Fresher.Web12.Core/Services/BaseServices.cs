using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;
using MISA.Fresher.Web12.Core.MISAAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Services
{
    public class BaseServices<T> : IBaseServices<T>
    {
        #region Dependency Injection

        private readonly IBaseRepository<T> _baseRepository;

        public BaseServices(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        #endregion

        #region Main Functions

        public int InsertService(T entity)
        {
            // Validate data from request
            // 1. General Validations
            BaseServices<T>.IsEmptyValidation(entity);
            IsDuplicatedValidation(entity, Guid.NewGuid(), false);

            // 2. Distinct Validations 

            // Everything is Okay!
            // Create a new entityId
            var properties = entity.GetType().GetProperties();
            if (properties[0] != null && properties[0].CanWrite)
            {
                properties[0].SetValue(entity, Guid.NewGuid());
            }

            int rowsEffect = _baseRepository.Insert(entity);

            return rowsEffect;
        }

        public int UpdateService(T entity, Guid entityId)
        {
            // Validate data from request
            // 1. General Validations
            BaseServices<T>.IsEmptyValidation(entity);
            IsDuplicatedValidation(entity, entityId, true);

            // 2. Distinct Validations

            // Everything is Okay!
            int rowsEffect = _baseRepository.UpdateById(entity, entityId);

            return rowsEffect;
        }

        #endregion

        #region Support Methods

        // General Validation
        // 1. Check if NotEmpty Props
        /// <summary>
        /// @author: Vũ Quang Phong (26/01/2022)
        /// @desc: Check if NotEmpty Props
        /// @edited: Vũ Quang Phong (28/01/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="MISAValidateException"></exception>
        private static void IsEmptyValidation(T entity)
        {
            // Getting properties marked not allowed Empty
            var notEmptyProps = entity.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(NotEmpty)));

            foreach (var prop in notEmptyProps)
            {
                // Getting the value of the Property
                var propValue = prop.GetValue(entity);

                // Getting PropsName of the Property
                var propsName = prop.GetCustomAttributes(typeof(PropsName), true);
                var propsNameDisplay = string.Empty;
                if (propsName.Length > 0)
                {
                    propsNameDisplay = ((PropsName)propsName[0]).Name;
                }

                if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                {
                    throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.PropNotEmpty, propsNameDisplay));
                }
            }
        }

        // 2. Check if NotDuplicated Props
        /// <summary>
        /// @author: Vũ Quang Phong (26/01/2022)
        /// @desc: Check if NotDuplicated Props
        /// @edited: Vũ Quang Phong (28/01/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <param name="isPut"></param>
        /// <exception cref="MISAValidateException"></exception>
        private void IsDuplicatedValidation(T entity, Guid entityId, bool isPut)
        {
            // Getting properties marked not allowed Duplicated
            var notDuplicatedProps = entity.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(NotDuplicated)));

            foreach (var prop in notDuplicatedProps)
            {
                // Getting the value of the Property
                var propValue = prop.GetValue(entity);

                // Getting PropsName of the Property
                var propsName = prop.GetCustomAttributes(typeof(PropsName), true);
                var propsNameDisplay = string.Empty;
                if (propsName.Length > 0)
                {
                    propsNameDisplay = ((PropsName)propsName[0]).Name;
                }

                if (_baseRepository.IsDuplicateCode(propValue.ToString(), entityId.ToString(), isPut))
                {
                    throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.PropNotDuplicated, propsNameDisplay));
                }
            }
        }

        // Distinct Validation
        // protected virtual void name_function(params) { ... }

        #endregion
    }
}
