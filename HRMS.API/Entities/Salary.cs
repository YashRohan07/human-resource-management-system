namespace HRMS.API.Entities;

// Current salary configuration
public class Salary : BaseEntity
{
    public int EmployeeId { get; set; }

    public decimal BasicSalary { get; set; }

    public decimal Bonus { get; set; }

    public decimal Deduction { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public Employee Employee { get; set; } = null!;
}