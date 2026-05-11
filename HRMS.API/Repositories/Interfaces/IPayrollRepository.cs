using HRMS.API.Data.QueryModels;
using HRMS.API.DTOs.Payrolls;
using HRMS.API.Entities;

namespace HRMS.API.Repositories.Interfaces;

// Database operations for payroll records
public interface IPayrollRepository
{
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync();

    Task<IEnumerable<Salary>> GetSalariesForEmployeesAsync(
        IEnumerable<int> employeeIds);

    Task<bool> ExistsAsync(
        int employeeId,
        int month,
        int year);

    Task AddRangeAsync(IEnumerable<Payroll> payrolls);

    Task<(IEnumerable<Payroll> Payrolls, int TotalCount)> GetAllAsync(
        PayrollQueryDto query);

    Task<Payroll?> GetByIdAsync(int id);

    Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(int employeeId);

    Task<IEnumerable<PayrollSummaryResult>> GetSummaryAsync(
        int month,
        int year);
}