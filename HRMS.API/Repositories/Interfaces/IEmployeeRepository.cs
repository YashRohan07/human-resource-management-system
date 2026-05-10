using HRMS.API.DTOs.Employees;
using HRMS.API.Entities;

namespace HRMS.API.Repositories.Interfaces;

// Database operations for employee records
public interface IEmployeeRepository
{
    Task<(IEnumerable<Employee> Employees, int TotalCount)> GetAllAsync(
        EmployeeQueryDto query);

    Task<Employee?> GetByIdAsync(int id);

    Task<Employee> CreateAsync(Employee employee);

    Task UpdateAsync(Employee employee);

    Task SoftDeleteAsync(Employee employee);

    Task<bool> ExistsByEmailAsync(string email, int? excludeEmployeeId = null);
}