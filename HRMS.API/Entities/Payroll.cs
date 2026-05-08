namespace HRMS.API.Entities;

// Monthly payroll snapshot
public class Payroll : BaseEntity
{
    public int EmployeeId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public decimal BasicSalarySnapshot { get; set; }

    public decimal BonusSnapshot { get; set; }

    public decimal DeductionSnapshot { get; set; }

    public decimal Tax { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal NetSalary { get; set; }

    public DateTime GeneratedAt { get; set; }

    public Employee Employee { get; set; } = null!;
}