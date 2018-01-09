namespace EmployeeService.Model
{
    /// <summary>
    /// This class contains fields holding Salary information
    /// </summary>
    public class Salary
    {
        public int EmployeeId { get; set; }
        public float grossPay { get; set; }
        public float stateTax { get; set; }
        public float federalTax { get; set; }
        public float healthInsurance { get; set; }
        public float socialSecurityTax { get; set; }
        public float reimbursements { get; set; }
        public float bonus { get; set; }

        public float payableSalary { get; set; }

    }
}