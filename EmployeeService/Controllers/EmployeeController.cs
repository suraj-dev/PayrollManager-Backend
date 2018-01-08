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
            try {
                return Ok(ManageEmployee.getAllEmployees());
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        // GET api/employee/5
        //public IHttpActionResult Get(int id)
        //{
        //    try {
        //        return Ok(ManageEmployee.GetDetails(id));
        //    }
        //    catch(Exception e)
        //    {
        //        return BadRequest();
        //    }
        //}

        // POST api/employee
        public IHttpActionResult Post( [FromBody]Employee emp)
        {
            
            try
            {
                float result = ManageEmployee.createEmployee(emp);
                if(result != -1)
                    return Ok(result);
                return Content(HttpStatusCode.Conflict, "Employee record already exists");
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        // PUT api/employee/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/employee/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                ManageEmployee.deleteEmployee(id);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
    }
}
