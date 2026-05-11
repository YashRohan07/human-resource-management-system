namespace HRMS.API.DTOs.Payrolls;

// Payroll response data
public class PayrollResponseDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public int Month { get; set; }

    public int Year { get; set; }

    public decimal BasicSalarySnapshot { get; set; }

    public decimal BonusSnapshot { get; set; }

    public decimal DeductionSnapshot { get; set; }

    public decimal Tax { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal NetSalary { get; set; }

    public DateTime GeneratedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}