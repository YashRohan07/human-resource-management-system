using HRMS.API.Common;
using HRMS.API.DTOs.Payrolls;
using HRMS.API.Data.QueryModels;

namespace HRMS.API.Services.Interfaces;

// Business operations for payroll module
public interface IPayrollService
{
    Task<IEnumerable<PayrollResponseDto>> GenerateAsync(
        PayrollGenerateDto request);

    Task<PagedResult<PayrollResponseDto>> GetAllAsync(
        PayrollQueryDto query);

    Task<PayrollResponseDto> GetByIdAsync(int id);

    Task<IEnumerable<PayrollResponseDto>> GetByEmployeeIdAsync(
        int employeeId);

    Task<IEnumerable<PayrollSummaryResult>> GetSummaryAsync(
        int month,
        int year);
}