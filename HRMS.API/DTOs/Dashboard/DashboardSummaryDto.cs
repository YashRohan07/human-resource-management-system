using HRMS.API.Data.QueryModels;

namespace HRMS.API.DTOs.Dashboard;

// Dashboard summary response
public class DashboardSummaryDto
{
    public int TotalEmployees { get; set; }

    public int ActiveEmployees { get; set; }

    public decimal CurrentMonthPayrollCost { get; set; }

    public IEnumerable<DepartmentEmployeeSummaryDto> DepartmentEmployeeCounts
    {
        get; set;
    } = [];

    public IEnumerable<PayrollSummaryResult> DepartmentPayrollSummaries
    {
        get; set;
    } = [];
}

// Employee count grouped by department
public class DepartmentEmployeeSummaryDto
{
    public string Department { get; set; } = string.Empty;

    public int TotalEmployees { get; set; }
}