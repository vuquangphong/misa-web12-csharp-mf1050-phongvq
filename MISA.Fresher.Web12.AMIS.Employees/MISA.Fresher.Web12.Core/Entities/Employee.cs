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
        public Guid? EmployeeId { get; set; }

        // Employee Code
        public string? EmployeeCode { get; set; }

        // First name of Employee
        public string? FirstName { get; set; }

        // Family name of Employee
        public string? LastName { get; set; }

        // Full name of Employee
        public string? EmployeeName { get; set; }

        // Foreign Key form Department
        public Guid? DepartmentId { get; set; }

        // Foreign Key form Position
        public Guid? PositionId { get; set; }

        /// <summary>
        /// Gender of Employee
        /// Gender = 0 --> Female
        /// Gender = 1 --> Male
        /// </summary>
        public int? Gender { get; set; }

        // Phone number of Employee
        public string? PhoneNumber { get; set; }

        // Landline number of Employee
        public string? TelephoneNumber { get; set; }

        // Email of Employee
        public string? Email { get; set; }

        // Address of Employee
        public string? Address { get; set; }

        // Date of birth of Employee
        public DateTime? DateOfBirth { get; set; }

        public string? IdentityNumber { get; set; }

        public DateTime? IdentityDate { get; set; }

        public string? IdentityPlace { get; set; }

        public string? BankAccountNumber { get; set; }

        public string? BankName { get; set; }

        public string? BankBranchName { get; set; }

        public string? BankProvinceName { get; set; }

        /// <summary>
        /// 0 --> neither customer or supplier 
        /// 1 --> customer
        /// 2 --> supplier
        /// 3 --> both
        /// </summary>
        public int? CustomerOrSupplier { get; set; }

        #endregion

        #region Method

        #endregion
    }
}
