using HRMS.API.Entities;

namespace HRMS.API.Repositories.Interfaces;

// Database operations for salary records
public interface ISalaryRepository
{
    Task<Salary?> GetByEmployeeIdAsync(int employeeId);

    Task<Salary?> GetByIdAsync(int id);

    Task<Salary> CreateAsync(Salary salary);

    Task UpdateAsync(Salary salary);

    Task<bool> ExistsByEmployeeIdAsync(int employeeId);
}