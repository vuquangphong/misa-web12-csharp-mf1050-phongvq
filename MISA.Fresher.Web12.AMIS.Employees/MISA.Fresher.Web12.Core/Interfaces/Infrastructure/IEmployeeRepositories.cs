using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Web12.Core.Entities;

namespace MISA.Fresher.Web12.Core.Interfaces.Infrastructure
{
    public interface IEmployeeRepositories
    {
        public IEnumerable<Employee> GetAll();

        public Employee GetById(string employeeId);

        public int Insert(Employee employee);

        public int UpdateById(Employee employee, string employeeId);

        public bool IsDuplicateEmployeeCode(string employeeCode);

        public int DeleteById(string employeeId);
    }
}
