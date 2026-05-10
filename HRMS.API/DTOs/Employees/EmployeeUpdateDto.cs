namespace HRMS.API.DTOs.Employees;

// Request model for updating employee
public class EmployeeUpdateDto
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public string EmploymentStatus { get; set; } = string.Empty;
}