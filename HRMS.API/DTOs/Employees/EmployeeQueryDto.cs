namespace HRMS.API.DTOs.Employees;

// Query parameters for employee list
public class EmployeeQueryDto
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public string? Department { get; set; }

    public string? Status { get; set; }

    public string? SortBy { get; set; }

    public string? SortOrder { get; set; }
}