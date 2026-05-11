using HRMS.API.Data;
using HRMS.API.DTOs.Dashboard;
using HRMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Services;

// Handles dashboard summary business logic
public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        var currentDate = DateTime.UtcNow;

        var totalEmployees = await _context.Employees.CountAsync();

        var activeEmployees = await _context.Employees
            .CountAsync(x => x.EmploymentStatus == "Active");

        var currentMonthPayrollCost = await _context.Payrolls
            .Where(x =>
                x.Month == currentDate.Month &&
                x.Year == currentDate.Year)
            .SumAsync(x => (decimal?)x.NetSalary) ?? 0;

        var departmentEmployeeCounts = await _context.Employees
            .GroupBy(x => x.Department)
            .Select(group => new DepartmentEmployeeSummaryDto
            {
                Department = group.Key,
                TotalEmployees = group.Count()
            })
            .OrderBy(x => x.Department)
            .ToListAsync();

        var departmentPayrollSummaries = await _context
            .PayrollSummaryResults
            .FromSqlRaw(
                "EXEC sp_GetPayrollSummaryByMonth @Month = {0}, @Year = {1}",
                currentDate.Month,
                currentDate.Year)
            .ToListAsync();

        return new DashboardSummaryDto
        {
            TotalEmployees = totalEmployees,
            ActiveEmployees = activeEmployees,
            CurrentMonthPayrollCost = currentMonthPayrollCost,
            DepartmentEmployeeCounts = departmentEmployeeCounts,
            DepartmentPayrollSummaries = departmentPayrollSummaries
        };
    }
}