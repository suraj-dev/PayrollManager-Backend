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
    /// <summary>
    /// This class acts as a controller that contains methods for handling GET, POST and DELETE requests for Employees.
    /// It uses the Manage Employee service to perform operations on the database. 
    /// </summary>
    public class EmployeeController : ApiController
    {
        
        // GET api/employee
        /// <summary>
        /// This method uses the Manage Employee service to retrieve all employees from the database
        /// </summary>
        /// <returns>
        /// Employees data with 200 status on Success,
        /// Exception with 500 status on Failure
        /// </returns>
        public IHttpActionResult Get()
        {
            try {
                return Ok(ManageEmployee.getAllEmployees());
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
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
        /// <summary>
        /// This method retrieves the Employee object from the body of the incoming POST request and
        /// uses the Manage Employee service to insert the employee into the database. It also returns
        /// the calculated salary
        /// </summary>
        /// <param name="emp">
        /// Body of the incoming POST request
        /// </param>
        /// <returns>
        /// Calculated salary with 200 status on Success,
        /// Exception with 500 status on Failure
        /// </returns>
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
                return BadRequest(e.ToString());
            }
        }

        //// PUT api/employee/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/employee/5
        /// <summary>
        /// This method uses the Manage Employee service to delete an employee from the database with 
        /// the Employee Id received in the DELETE request.
        /// </summary>
        /// <param name="id">
        /// Employee Id extracted from the URL of the incoming DELETE request
        /// </param>
        /// <returns>
        /// 200 status on Success
        /// Exception with 500 status on Failure
        /// </returns>
        public IHttpActionResult Delete(int id)
        {
            try
            {
                ManageEmployee.deleteEmployee(id);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}
