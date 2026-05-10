using HRMS.API.Data;
using HRMS.API.DTOs.Employees;
using HRMS.API.Entities;
using HRMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Repositories;

// EF Core implementation for employee records
public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Employee> Employees, int TotalCount)> GetAllAsync(
        EmployeeQueryDto query)
    {
        var employeesQuery = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchText = query.Search.Trim();

            employeesQuery = employeesQuery.Where(x =>
                x.FullName.Contains(searchText) ||
                x.Email.Contains(searchText));
        }

        if (!string.IsNullOrWhiteSpace(query.Department))
        {
            var department = query.Department.Trim();

            employeesQuery = employeesQuery.Where(x =>
                x.Department == department);
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            var status = query.Status.Trim();

            employeesQuery = employeesQuery.Where(x =>
                x.EmploymentStatus == status);
        }

        employeesQuery = ApplySorting(employeesQuery, query);

        var totalCount = await employeesQuery.CountAsync();

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var employees = await employeesQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (employees, totalCount);
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Employee> CreateAsync(Employee employee)
    {
        _context.Employees.Add(employee);

        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);

        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(Employee employee)
    {
        employee.IsDeleted = true;

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        int? excludeEmployeeId = null)
    {
        var query = _context.Employees
            .Where(x => x.Email == email);

        if (excludeEmployeeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeEmployeeId.Value);
        }

        return await query.AnyAsync();
    }

    private static IQueryable<Employee> ApplySorting(
        IQueryable<Employee> query,
        EmployeeQueryDto employeeQuery)
    {
        var sortBy = employeeQuery.SortBy?.Trim().ToLower();
        var sortOrder = employeeQuery.SortOrder?.Trim().ToLower();

        var isDescending = sortOrder == "desc";

        return sortBy switch
        {
            "email" => isDescending
                ? query.OrderByDescending(x => x.Email)
                : query.OrderBy(x => x.Email),

            "department" => isDescending
                ? query.OrderByDescending(x => x.Department)
                : query.OrderBy(x => x.Department),

            "status" => isDescending
                ? query.OrderByDescending(x => x.EmploymentStatus)
                : query.OrderBy(x => x.EmploymentStatus),

            "createdat" => isDescending
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt),

            _ => isDescending
                ? query.OrderByDescending(x => x.FullName)
                : query.OrderBy(x => x.FullName)
        };
    }
}