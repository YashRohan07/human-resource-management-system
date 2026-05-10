namespace HRMS.API.DTOs.Salaries;

// Request model for creating employee salary
public class SalaryCreateDto
{
    public int EmployeeId { get; set; }

    public decimal BasicSalary { get; set; }

    public decimal Bonus { get; set; }

    public decimal Deduction { get; set; }

    public DateTime EffectiveFrom { get; set; }
}