using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using EmployeeService.Model;
namespace EmployeeService.Database
{

    public class ManageEmployee
    {
        private static string _conn = ConfigurationManager.ConnectionStrings["PayrollConnection"].ConnectionString;

        public static Employee GetDetails(int empId) {

            using (SqlConnection connection = new SqlConnection(_conn))
            {
                string cmdText = "select * from Employee where Id = @empId";
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                cmd.Parameters.AddWithValue("@empId", empId);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Employee emp = new Employee();

                while (reader.Read())
                {
                    emp.FirstName = reader["FirstName"].ToString();
                    emp.LastName = reader["LastName"].ToString();
                    emp.Email = reader["Email"].ToString();
                    emp.Address = reader["Address"].ToString();
                    emp.PhoneNumber = reader["PhoneNumber"].ToString();
                    emp.SSN = reader["SSN"].ToString();
                }
                return emp;
            }
        }

        public static List<Employee> getAllEmployees()
        {
            using (SqlConnection connection = new SqlConnection(_conn))
            {
                //string cmdText = @"select e.FirstName as firstName, 
                //                          e.LastName as lastName, 
                //                          e.Email as email,
                //                          e.Address as addr, 
                //                          e.PhoneNumber as phone,
                //                          e.SSN as ssn

                //    from Employee e, Salary s on e.Id = s.EmployeeId";
                string cmdText = @"select *, s.Id as salaryId from Employee e, Salary s where e.Id = s.EmployeeId";
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
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
                    emp.empSalary.reimbursements = Convert.ToInt32(reader["Reimbursements"]);
                    emp.empSalary.grossPay = Convert.ToInt32(reader["GrossPay"]);
                    emp.empSalary.stateTax = Convert.ToInt32(reader["StateTax"]);
                    emp.empSalary.federalTax = Convert.ToInt32(reader["FederalTax"]);
                    emp.empSalary.socialSecurityTax = Convert.ToInt32(reader["SocialSecurityTax"]);
                    emp.empSalary.healthInsurance = Convert.ToInt32(reader["HealthInsurance"]);
                    emp.empSalary.payableSalary = Convert.ToInt32(reader["PayableSalary"]);
                    employees.Add(emp);
                }
                return employees;
            }
        }

        public static float createEmployee(Employee emp)
        {
            Salary sal = emp.empSalary;
            float payableSalary = calculatePayableSalary(sal);
            return payableSalary;
        }

        public static float calculatePayableSalary(Salary sal)
        {
            float grossPay = sal.grossPay;
            float stateTax = (sal.stateTax/100)*grossPay;
            float federalTax = (sal.federalTax/100)*grossPay;
            float reimbursements = sal.reimbursements;
            float bonus = sal.bonus;
            float healthInsurance = (sal.healthInsurance / 100) * grossPay;
            float socialSecurityTax = (sal.socialSecurityTax / 100) * grossPay;

            float deductions = stateTax + federalTax + healthInsurance + socialSecurityTax;
            float payableSalary = grossPay - deductions + reimbursements + bonus;

            return payableSalary;

        }


    }
}