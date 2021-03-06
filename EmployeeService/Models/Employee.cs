﻿namespace EmployeeService.Model
{
    /// <summary>
    /// This class contains fields holding employee as well as Salary information
    /// </summary>
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string SSN { get; set; }
        public Salary empSalary { get; set; }
    }
}