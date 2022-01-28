using MISA.Fresher.Web12.Core.Enum;
using MISA.Fresher.Web12.Core.MISAAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [NotEmpty]
        [NotDuplicated]
        [PropsName("Mã nhân viên")]
        public string? EmployeeCode { get; set; }

        // First name of Employee
        public string? FirstName { get; set; }

        // Family name of Employee
        public string? LastName { get; set; }

        // Full name of Employee
        [NotEmpty]
        [PropsName("Tên nhân viên")]
        public string? EmployeeName { get; set; }

        // Foreign Key form Department
        [NotEmpty]
        [PropsName("Đơn vị")]
        public Guid? DepartmentId { get; set; }

        // Foreign Key form Position
        public Guid? PositionEId { get; set; }

        /// <summary>
        /// Gender of Employee
        /// Gender = 0 --> Female
        /// Gender = 1 --> Male
        /// </summary>
        public Gender? Gender { get; set; }

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

        // Identity Number of Employee
        public string? IdentityNumber { get; set; }

        // Identity Date of Employee
        public DateTime? IdentityDate { get; set; }

        // Identity Place of Employee
        public string? IdentityPlace { get; set; }

        // Bank Account Number of Employee
        public string? BankAccountNumber { get; set; }

        // Bank Name of Employee
        public string? BankName { get; set; }

        // Bank Branch Name of Employee
        public string? BankBranchName { get; set; }

        // Bank Province Name of Employee
        public string? BankProvinceName { get; set; }

        /// <summary>
        /// 0 --> neither customer or supplier 
        /// 1 --> customer
        /// 2 --> supplier
        /// 3 --> both
        /// </summary>
        public CustomerOrSupplier? CustomerOrSupplier { get; set; }

        #endregion

        #region Method

        #endregion
    }
}
