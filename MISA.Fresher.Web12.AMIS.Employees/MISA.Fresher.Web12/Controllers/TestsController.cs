using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Web12.Models;

namespace MISA.Fresher.Web12.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        /// <summary>
        /// This is just a normal GET...
        /// </summary>
        [HttpGet]
        public string Get()
        {
            return "GET: Vũ Quang Phong";
        }


        /// <summary>
        /// There are some GETs/POSTs that use parameters...
        /// </summary>
         
        // Passing params through Router
        [HttpGet("{name}")]
        public string GetName([FromRoute]string name)
        {
            return $"GET: {name}";
        }

        // Passing params through Router
        [HttpGet("{name}/{address}")]
        public string GetNameAndAddress(string name, string address)
        {
            return $"GET: My name is {name}. I come from {address}";
        }

        // Passing params through Query String
        //[HttpGet("search")]
        //public string GetNameByAge([FromQuery] int? age)
        //{
        //    if (age < 18) return "GET: Windy from childhood";
        //    return "GET: Vũ Quang Phong from MISA";
        //}

        // Passing params through Query String
        [HttpGet("search")]
        public string getNameByAgeAndAddress(int? age, string address)
        {
            if (age < 18 && address == "Bắc Ninh") return "GET: Bắc Ninh 99 hello everyone, I am Windy from childhood";
            return "GET: Vũ Quang Phong from MISA";
        }

        // Passing params through Body request (json)
        [HttpPost]
        public Employee Post(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid();
            return employee;
        }
    }
}
