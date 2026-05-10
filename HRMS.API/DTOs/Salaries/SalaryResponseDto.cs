namespace HRMS.API.DTOs.Salaries;

// Response model for salary APIs
public class SalaryResponseDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public decimal BasicSalary { get; set; }

    public decimal Bonus { get; set; }

    public decimal Deduction { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}