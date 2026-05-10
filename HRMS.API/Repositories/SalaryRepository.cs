using HRMS.API.Data;
using HRMS.API.Entities;
using HRMS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Repositories;

// EF Core implementation for salary records
public class SalaryRepository : ISalaryRepository
{
    private readonly ApplicationDbContext _context;

    public SalaryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Salary?> GetByEmployeeIdAsync(int employeeId)
    {
        return await _context.Salaries
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
    }

    public async Task<Salary?> GetByIdAsync(int id)
    {
        return await _context.Salaries
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Salary> CreateAsync(Salary salary)
    {
        _context.Salaries.Add(salary);

        await _context.SaveChangesAsync();

        return salary;
    }

    public async Task UpdateAsync(Salary salary)
    {
        _context.Salaries.Update(salary);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByEmployeeIdAsync(int employeeId)
    {
        return await _context.Salaries
            .AnyAsync(x => x.EmployeeId == employeeId);
    }
}