using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using EmployeeService.Model;
namespace EmployeeService.Database
{

    /// <summary>
    /// This class contains methods that access the database to perform operations on Employee and Salary tables
    /// </summary>
    public class ManageEmployee
    {
        /// <summary>
        /// Connection string containing information about connecting to the database
        /// </summary>
        private static string _conn = ConfigurationManager.ConnectionStrings["PayrollConnection"].ConnectionString;

        //public static Employee GetDetails(int empId) {

        //    using (SqlConnection connection = new SqlConnection(_conn))
        //    {
        //        string cmdText = "select * from Employee where Id = @empId";
        //        SqlCommand cmd = new SqlCommand(cmdText, connection);
        //        cmd.Parameters.AddWithValue("@empId", empId);
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        Employee emp = new Employee();

        //        while (reader.Read())
        //        {
        //            emp.FirstName = reader["FirstName"].ToString();
        //            emp.LastName = reader["LastName"].ToString();
        //            emp.Email = reader["Email"].ToString();
        //            emp.Address = reader["Address"].ToString();
        //            emp.PhoneNumber = reader["PhoneNumber"].ToString();
        //            emp.SSN = reader["SSN"].ToString();
        //        }
        //        return emp;
        //    }
        //}

            /// <summary>
            /// This method connects to the database and retrieves all the employees and their salary information
            /// </summary>
            /// <returns>
            /// List of employee objects
            /// </returns>
        public static List<Employee> getAllEmployees()
        {
            using (ConnectionService cs = new ConnectionService(_conn))
            {
                string cmdText = @"select *, s.Id as salaryId from Employee e, Salary s where e.Id = s.EmployeeId";
                SqlCommand cmd = cs.getSQLCommand(cmdText);

                cs.connection.Open();
                SqlDataReader reader = cs.getSQLReader();

                List<Employee> employees = new List<Employee>();
                float bonus = 0.0f;
                float reimbursements = 0.0f;
                float stateTax = 0.0f;
                float federalTax = 0.0f;
                float healthInsurance = 0.0f;
                float grossPay = 0.0f;
                float socialSecurityTax = 0.0f;
                float payableSalary = 0.0f;

                while (reader.Read())
                {

                    Employee emp = new Employee();
                    emp.empSalary = new Salary();
                    string id = reader["salaryId"].ToString();
                    emp.FirstName = reader["FirstName"].ToString();
                    emp.LastName = reader["LastName"].ToString();
                    emp.Email = reader["Email"].ToString();
                    emp.Address = reader["Address"].ToString();
                    emp.PhoneNumber = reader["PhoneNumber"].ToString();
                    emp.SSN = reader["SSN"].ToString();
                    emp.empSalary.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                    float.TryParse(reader["Bonus"].ToString(), out bonus);
                    emp.empSalary.bonus = bonus;
                    float.TryParse(reader["Reimbursements"].ToString(), out reimbursements);
                    emp.empSalary.reimbursements = reimbursements;
                    float.TryParse(reader["GrossPay"].ToString(), out grossPay);
                    emp.empSalary.grossPay = grossPay;
                    float.TryParse(reader["StateTax"].ToString(), out stateTax);
                    emp.empSalary.stateTax = stateTax;
                    float.TryParse(reader["FederalTax"].ToString(), out federalTax);
                    emp.empSalary.federalTax = federalTax;
                    float.TryParse(reader["SocialSecurityTax"].ToString(), out socialSecurityTax);
                    emp.empSalary.socialSecurityTax = socialSecurityTax;
                    float.TryParse(reader["HealthInsurance"].ToString(), out healthInsurance);
                    emp.empSalary.healthInsurance = healthInsurance;
                    float.TryParse(reader["PayableSalary"].ToString(), out payableSalary);
                    emp.empSalary.payableSalary = payableSalary;
                    employees.Add(emp);
                }
                return employees;


            }
        }

        /// <summary>
        /// This method connects to the database and inserts the received employee object into the database if there
        /// are no other employee records present with the same information.
        /// </summary>
        /// <param name="emp">
        /// Employee object received from the Employee controller
        /// </param>
        /// <returns>
        /// Salary calculated based on the information received
        /// </returns>
        public static float createEmployee(Employee emp)
        {

            Salary sal = emp.empSalary;
            float payableSalary = 0.0f;
            int count = 0;
            using (ConnectionService cs = new ConnectionService(_conn))
            {
                string cmdText = @"select COUNT(*) from Employee where Email=@email OR SSN=@ssn OR PhoneNumber=@phoneNo";
                SqlCommand cmnd = cs.getSQLCommand(cmdText);
                cmnd.Parameters.AddWithValue("@email", emp.Email);
                cmnd.Parameters.AddWithValue("@ssn", emp.SSN);
                cmnd.Parameters.AddWithValue("@phoneNo", emp.PhoneNumber);
                cs.connection.Open();
                count = (int)cmnd.ExecuteScalar();
                if (count > 0)
                {
                    return -1;
                }

            }

            using (ConnectionService cs = new ConnectionService(_conn))
            {
                string commandText = "CreateEmployee";
                SqlCommand cmd = cs.getSQLCommand(commandText);
                payableSalary = calculatePayableSalary(sal);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
                cmd.Parameters.AddWithValue("@LastName", emp.LastName);
                cmd.Parameters.AddWithValue("@Email", emp.Email);
                cmd.Parameters.AddWithValue("@Address", emp.Address);
                cmd.Parameters.AddWithValue("@PhoneNumber", emp.PhoneNumber);
                cmd.Parameters.AddWithValue("@SSN", emp.SSN);
                cmd.Parameters.AddWithValue("@GrossPay", emp.empSalary.grossPay);
                cmd.Parameters.AddWithValue("@StateTax", emp.empSalary.stateTax);
                cmd.Parameters.AddWithValue("@FederalTax", emp.empSalary.federalTax);
                cmd.Parameters.AddWithValue("@SocialSecurityTax", emp.empSalary.socialSecurityTax);
                cmd.Parameters.AddWithValue("@Bonus", emp.empSalary.bonus);
                cmd.Parameters.AddWithValue("@Reimbursements", emp.empSalary.reimbursements);
                cmd.Parameters.AddWithValue("@HealthInsurance", emp.empSalary.healthInsurance);
                cmd.Parameters.AddWithValue("@PayableSalary", payableSalary);
                cs.connection.Open();
                cmd.ExecuteNonQuery();

                return payableSalary;

            }
        }

        /// <summary>
        /// This method calculates the payable salary with the information received from the Salary object
        /// </summary>
        /// <param name="sal">
        /// Salary object 
        /// </param>
        /// <returns>
        /// Payable salary
        /// </returns>
        public static float calculatePayableSalary(Salary sal)
        {
            float grossPay = sal.grossPay;
            float stateTax = (sal.stateTax / 100) * grossPay;
            float federalTax = (sal.federalTax / 100) * grossPay;
            float reimbursements = sal.reimbursements;
            float bonus = sal.bonus;
            float healthInsurance = (sal.healthInsurance / 100) * grossPay;
            float socialSecurityTax = (sal.socialSecurityTax / 100) * grossPay;

            float deductions = stateTax + federalTax + healthInsurance + socialSecurityTax;
            float payableSalary = grossPay - deductions + reimbursements + bonus;

            return payableSalary;

        }

        /// <summary>
        /// This method connects to the database and deletes the employee with the received Id from the database.
        /// </summary>
        /// <param name="id">
        /// Employee Id
        /// </param>
        public static void deleteEmployee(int id)
        {
            using (ConnectionService cs = new ConnectionService(_conn))
            {
                string cmdText = @"delete from Employee where Id=@empId";
                SqlCommand cmd = cs.getSQLCommand(cmdText);
                cmd.Parameters.AddWithValue("@empId", id);
                cs.connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}