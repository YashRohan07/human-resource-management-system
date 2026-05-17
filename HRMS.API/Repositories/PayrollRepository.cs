using HRMS.API.Data;
using HRMS.API.Data.QueryModels;
using HRMS.API.DTOs.Payrolls;
using HRMS.API.Entities;
using HRMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Repositories;

// EF Core implementation for payroll records
public class PayrollRepository : IPayrollRepository
{
    private readonly ApplicationDbContext _context;

    public PayrollRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        return await _context.Employees
            .Where(x => x.EmploymentStatus == "Active")
            .ToListAsync();
    }

    public async Task<IEnumerable<Salary>> GetSalariesForEmployeesAsync(
        IEnumerable<int> employeeIds)
    {
        return await _context.Salaries
            .Where(x => employeeIds.Contains(x.EmployeeId))
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(
        int employeeId,
        int month,
        int year)
    {
        return await _context.Payrolls.AnyAsync(x =>
            x.EmployeeId == employeeId &&
            x.Month == month &&
            x.Year == year);
    }

    public async Task AddRangeAsync(IEnumerable<Payroll> payrolls)
    {
        _context.Payrolls.AddRange(payrolls);

        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Payroll> Payrolls, int TotalCount)> GetAllAsync(
        PayrollQueryDto query)
    {
        var payrollsQuery = _context.Payrolls
            .IgnoreQueryFilters()
            .Include(x => x.Employee)
            .AsQueryable();

        if (query.Month.HasValue)
        {
            payrollsQuery = payrollsQuery.Where(x =>
                x.Month == query.Month.Value);
        }

        if (query.Year.HasValue)
        {
            payrollsQuery = payrollsQuery.Where(x =>
                x.Year == query.Year.Value);
        }

        if (query.EmployeeId.HasValue)
        {
            payrollsQuery = payrollsQuery.Where(x =>
                x.EmployeeId == query.EmployeeId.Value);
        }

        payrollsQuery = payrollsQuery
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .ThenBy(x => x.Employee.FullName);

        var totalCount = await payrollsQuery.CountAsync();

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var payrolls = await payrollsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (payrolls, totalCount);
    }

    public async Task<Payroll?> GetByIdAsync(int id)
    {
        return await _context.Payrolls
            .IgnoreQueryFilters()
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(
        int employeeId)
    {
        return await _context.Payrolls
            .IgnoreQueryFilters()
            .Include(x => x.Employee)
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .ToListAsync();
    }

    public async Task<IEnumerable<PayrollSummaryResult>> GetSummaryAsync(
        int month,
        int year)
    {
        return await _context.PayrollSummaryResults
            .FromSqlRaw(
                "EXEC sp_GetPayrollSummaryByMonth @Month = {0}, @Year = {1}",
                month,
                year)
            .ToListAsync();
    }
}