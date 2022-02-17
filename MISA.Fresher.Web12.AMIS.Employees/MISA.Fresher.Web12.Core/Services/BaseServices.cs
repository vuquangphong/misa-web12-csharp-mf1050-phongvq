using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;
using MISA.Fresher.Web12.Core.MISAAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public IEnumerable<T> GetAllService()
        {
            return _baseRepository.GetAll();
        }

        public T GetByIdService(string entityId)
        {
            return _baseRepository.GetById(entityId);
        }

        public int InsertService(T entity)
        {
            // Validate data from request
            // 1. General Validations
            BaseServices<T>.IsEmptyValidation(entity);
            BaseServices<T>.IsFormatCode(entity);
            IsDuplicatedValidation(entity, Guid.NewGuid(), false);

            BaseServices<T>.IsGreaterCurrentDate(entity);
            BaseServices<T>.IsFormatEmail(entity);

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
            BaseServices<T>.IsFormatCode(entity);
            IsDuplicatedValidation(entity, entityId, true);

            BaseServices<T>.IsGreaterCurrentDate(entity);
            BaseServices<T>.IsFormatEmail(entity);

            // 2. Distinct Validations

            // Everything is Okay!
            int rowsEffect = _baseRepository.UpdateById(entity, entityId);

            return rowsEffect;
        }

        public int DeleteService(string entityId)
        {
            return _baseRepository.DeleteById(entityId);
        }

        public int DeleteMultiService(string[] entityIds)
        {
            return _baseRepository.DeleteMultiById(entityIds);
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
            var notEmptyProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotEmpty)));

            foreach (var prop in notEmptyProps)
            {
                // Getting the value of the Property
                var propValue = prop.GetValue(entity);

                // Getting PropsName of the Property
                var propsName = prop.GetCustomAttributes(typeof(PropsName), true);
                var propNameDisplay = string.Empty;
                if (propsName.Length > 0)
                {
                    propNameDisplay = ((PropsName)propsName[0]).Name;
                }

                if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                {
                    throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.PropNotEmpty, propNameDisplay));
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
            var notDuplicatedProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotDuplicated)));

            foreach (var prop in notDuplicatedProps)
            {
                // Getting the value of the Property
                var propValue = prop.GetValue(entity);

                // Getting PropsName of the Property
                var propsName = prop.GetCustomAttributes(typeof(PropsName), true);
                var propNameDisplay = string.Empty;
                if (propsName.Length > 0)
                {
                    propNameDisplay = ((PropsName)propsName[0]).Name;
                }

                if (_baseRepository.IsDuplicateCode(propValue.ToString(), entityId.ToString(), isPut))
                {
                    throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.PropNotDuplicated, propNameDisplay));
                }
            }
        }

        // Format other Props (code, email, date,...)
        /// <summary>
        /// @author: Vũ Quang Phong (11/02/2022)
        /// @desc: Format Employee Code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="nameDisplay"></param>
        /// <exception cref="MISAValidateException"></exception>
        private static void IsFormatCode(T entity)
        {
            // Getting props marked FormatCode
            var formatCodeProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(FormatCode)));

            // Regex Pattern for formatting code
            var regex = new Regex(@"\A(NV-)+([0-9]{7})\Z");

            foreach (var prop in formatCodeProps)
            {
                var propValue = prop.GetValue(entity);

                var propsName = prop.GetCustomAttributes(typeof(PropsName), true);
                var propNameDisplay = string.Empty;
                if (propsName.Length > 0)
                {
                    propNameDisplay = ((PropsName)propsName[0]).Name;
                }

                if (!regex.IsMatch(propValue.ToString()))
                {
                    throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.WrongFormat, propNameDisplay));
                }
            }
            
        }

        /// <summary>
        /// @author: Vũ Quang Phong (11/02/2022)
        /// @desc: Format Email
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="MISAValidateException"></exception>
        private static void IsFormatEmail(T entity)
        {
            // Getting props marked FormatEmail
            var formatEmailProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(FormatEmail)));

            // Regex Pattern for formatting Email
            var regex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

            foreach (var prop in formatEmailProps)
            {
                var propValue = prop.GetValue(entity);

                if (propValue != null && !string.IsNullOrEmpty(propValue.ToString()))
                {
                    if (!regex.IsMatch(propValue.ToString()))
                    {
                        throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.WrongFormat, "Email"));
                    }
                }
            }
        }

        /// <summary>
        /// @author: Vũ Quang Phong (11/02/2022)
        /// @desc: Format Date (the date must be less than current date)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="nameDisplay"></param>
        /// <exception cref="MISAValidateException"></exception>
        private static void IsGreaterCurrentDate(T entity)
        {
            // Getting props marked FormatDate
            var formatDateProps = entity.GetType().GetProperties().Where(property => Attribute.IsDefined(property, typeof(FormatDate)));

            // Create the current Date
            DateTime currentDate = DateTime.Now;

            foreach (var prop in formatDateProps)
            {
                var propValue = prop.GetValue(entity);

                if (propValue != null && !string.IsNullOrEmpty(propValue.ToString())) 
                {
                    var propsName = prop.GetCustomAttributes(typeof(PropsName), true);
                    var propNameDisplay = string.Empty;
                    if (propsName.Length > 0)
                    {
                        propNameDisplay = ((PropsName)propsName[0]).Name;
                    }

                    if ((DateTime)propValue > currentDate)
                    {
                        throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.WrongDateFormat, propNameDisplay));
                    }
                }
            }
        }

        // Distinct Validation
        // protected virtual void name_function(params) { ... }

        #endregion
    }
}
