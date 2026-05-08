namespace HRMS.API.Data.QueryModels;

// Used only for payroll summary query results
public class PayrollSummaryResult
{
    public string Department { get; set; } = string.Empty;

    public int TotalEmployees { get; set; }

    public decimal TotalSalary { get; set; }
}