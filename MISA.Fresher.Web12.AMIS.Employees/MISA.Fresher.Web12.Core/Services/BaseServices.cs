﻿using MISA.Fresher.Web12.Core.Exceptions;
using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;
using MISA.Fresher.Web12.Core.MISAAttributes;
using MISA.Fresher.Web12.Core.OtherModels;
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

        public ControllerResponseData GetAllData()
        {
            var data = _baseRepository.GetAll();

            var res = new ControllerResponseData
            {
                customStatusCode = (int?)(data == null || data?.Count() == 0 ? Core.Enum.CustomizeStatusCode.NoContent : Core.Enum.CustomizeStatusCode.GetOkay),
                responseData = data
            };

            return res;
        }

        public ControllerResponseData GetDataById(string entityId)
        {
            var data = _baseRepository.GetById(entityId);

            var res = new ControllerResponseData
            {
                customStatusCode = (int?)(data == null ? Core.Enum.CustomizeStatusCode.NoContent : Core.Enum.CustomizeStatusCode.GetOkay),
                responseData = data
            };

            return res;
        }

        public ControllerResponseData InsertData(T entity)
        {
            // Validate data from request
            // 1. General Validations
            this.EmptyValidation(entity);
            this.FormatCodeValidation(entity);
            this.DuplicatedValidation(entity, Guid.NewGuid(), false);
            this.GreaterCurrentDateValidation(entity);
            this.FormatEmailValidation(entity);

            // 2. Distinct Validations 

            // Everything is Okay!
            // Create a new entityId
            var primaryKeyProp = entity.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PrimaryKey))    
            );

            foreach (var prop in primaryKeyProp)
            {
                prop.SetValue(entity, Guid.NewGuid());
            }

            int rowsEffect = _baseRepository.Insert(entity);

            var res = new ControllerResponseData
            {
                customStatusCode = (int?)(rowsEffect > 0 ? Core.Enum.CustomizeStatusCode.Created : Core.Enum.CustomizeStatusCode.NoContent),
                responseData = rowsEffect,
            };

            return res;
        }

        public ControllerResponseData UpdateData(T entity, Guid entityId)
        {
            // Validate data from request
            // 1. General Validations
            this.EmptyValidation(entity);
            this.FormatCodeValidation(entity);
            this.DuplicatedValidation(entity, entityId, true);
            this.GreaterCurrentDateValidation(entity);
            this.FormatEmailValidation(entity);

            // 2. Distinct Validations

            // Everything is Okay
            int rowsEffect = _baseRepository.UpdateById(entity, entityId);

            var res = new ControllerResponseData
            {
                customStatusCode = (int?)(rowsEffect > 0 ? Core.Enum.CustomizeStatusCode.Updated : Core.Enum.CustomizeStatusCode.NoContent),
                responseData = rowsEffect,
            };

            return res;
        }

        public ControllerResponseData DeleteData(string entityId)
        {
            int rowsEffect = _baseRepository.DeleteById(entityId);

            var res = new ControllerResponseData
            {
                customStatusCode = (int?)(rowsEffect > 0 ? Core.Enum.CustomizeStatusCode.Deleted : Core.Enum.CustomizeStatusCode.NoContent),
                responseData = rowsEffect,
            };

            return res;
        }

        public ControllerResponseData DeleteMultiData(string[] entityIds)
        {
            int rowsEffect = _baseRepository.DeleteMultiById(entityIds);

            var res = new ControllerResponseData
            {
                customStatusCode = (int?)(rowsEffect > 0 ? Core.Enum.CustomizeStatusCode.Deleted : Core.Enum.CustomizeStatusCode.NoContent),
                responseData = rowsEffect,
            };

            return res;
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
        private void EmptyValidation(T entity)
        {
            // Getting properties marked not allowed Empty
            var notEmptyProps = entity.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(NotEmpty))
            );

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
                    // Method 1: Responding messages respectively
                    throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.PropNotEmpty, propNameDisplay));

                    // Method 2: Responding all at once
                    //_listErrMsgs.Add(String.Format(Core.Resources.ResourceVietnam.PropNotEmpty, propNameDisplay));

                    // Method 3: Do not throw Exception
                    // TODO
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
        private void DuplicatedValidation(T entity, Guid entityId, bool isPut)
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
                    // Method 1: Responding messages respectively
                    throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.PropNotDuplicated, propNameDisplay));

                    // Method 2: Responding all at once
                    //_listErrMsgs.Add(String.Format(Core.Resources.ResourceVietnam.PropNotDuplicated, propNameDisplay));

                    // Method 3: Do not throw Exception
                    // TODO
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
        private void FormatCodeValidation(T entity)
        {
            // Getting props marked FormatCode
            var formatCodeProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(FormatCode)));

            // Regex Pattern for formatting code
            var regex = new Regex(@"\A(NV-)+([0-9]{4})\Z");

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
                    // Method 1: Responding messages respectively
                    throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.WrongFormat, propNameDisplay));

                    // Method 2: Responding all at once
                    //_listErrMsgs.Add(String.Format(Core.Resources.ResourceVietnam.WrongFormat, propNameDisplay));

                    // Method 3: Do not throw Exception
                    // TODO
                }
            }
        }

        /// <summary>
        /// @author: Vũ Quang Phong (11/02/2022)
        /// @desc: Format Email
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="MISAValidateException"></exception>
        private void FormatEmailValidation(T entity)
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
                        // Method 1: Responding messages respectively
                        throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.WrongFormat, "Email"));

                        // Method 2: Responding all at once
                        //_listErrMsgs.Add(String.Format(Core.Resources.ResourceVietnam.WrongFormat, "Email"));

                        // Method 3: Do not throw Exception
                        // TODO
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
        private void GreaterCurrentDateValidation(T entity)
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
                        // Method 1: Responding messages respectively
                        throw new MISAValidateException(String.Format(Core.Resources.ResourceVietnam.WrongDateFormat, propNameDisplay));

                        // Method 2: Responding all at once
                        //_listErrMsgs.Add(String.Format(Core.Resources.ResourceVietnam.WrongDateFormat, propNameDisplay));

                        // Method 3: Do not throw Exception
                        // TODO
                    }
                }
            }
        }

        // Distinct Validation
        // protected virtual void name_function(params) { ... }

        #endregion
    }
}
