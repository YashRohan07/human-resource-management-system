namespace HRMS.API.DTOs.Salaries;

// Request model for updating existing salary
public class SalaryUpdateDto
{
    public decimal BasicSalary { get; set; }

    public decimal Bonus { get; set; }

    public decimal Deduction { get; set; }

    public DateTime EffectiveFrom { get; set; }
}