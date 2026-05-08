namespace HRMS.API.Entities;

// HR employee record
public class Employee : SoftDeletableEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    // Active, Inactive, Terminated
    public string EmploymentStatus { get; set; } = "Active";

    // One-to-one relationship
    public Salary? Salary { get; set; }

    // One employee can have many payrolls
    public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
}