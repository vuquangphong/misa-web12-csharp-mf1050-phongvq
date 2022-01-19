using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Web12.Core.Entities
{
    public class Employee
    {
        #region Constructor
        public Employee()
        {

        }
        #endregion

        #region Properties

        // Primary Key
        public Guid EmployeeId { get; set; }

        // Employee Code
        public string? EmployeeCode { get; set; }

        // First name of Employee
        public string? FirstName { get; set; }

        // Family name of Employee
        public string? LastName { get; set; }

        // Full name of Employee
        public string? FullName { get; set; }

        /// <summary>
        /// Gender of Employee
        /// Gender = 0 --> Female
        /// Gender = 1 --> Male
        /// </summary>
        public int? Gender { get; set; }

        // Phone number of Employee
        public string? PhoneNumber { get; set; }

        // Email of Employee
        public string? Email { get; set; }

        // Address of Employee
        public string? Address { get; set; }

        // Date of birth of Employee
        public DateTime? DateOfBirth { get; set; }

        #endregion

        #region Method

        #endregion
    }
}
