using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using EmployeeService.Model;
using EmployeeService.Database;

namespace EmployeeService.Controllers
{
  //  [Authorize]
    public class EmployeeController : ApiController
    {
        // GET api/employee
        public IHttpActionResult Get()
        {
            return Ok(ManageEmployee.getAllEmployees());
        }

        // GET api/employee/5
        public Employee Get(int id)
        {
            return ManageEmployee.GetDetails(id);
        }

        // POST api/values
        public IHttpActionResult Post( [FromBody]Employee emp)
        {
            try
            {
                return Ok(ManageEmployee.createEmployee(emp));
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
