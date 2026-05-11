namespace HRMS.API.DTOs.Payrolls;

// Query parameters for payroll list
public class PayrollQueryDto
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public int? Month { get; set; }

    public int? Year { get; set; }

    public int? EmployeeId { get; set; }
}