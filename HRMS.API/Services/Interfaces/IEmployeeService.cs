using HRMS.API.Common;
using HRMS.API.DTOs.Employees;

namespace HRMS.API.Services.Interfaces;

// Business operations for employees
public interface IEmployeeService
{
    Task<PagedResult<EmployeeResponseDto>> GetAllAsync(
        EmployeeQueryDto query);

    Task<EmployeeResponseDto> GetByIdAsync(int id);

    Task<EmployeeResponseDto> CreateAsync(
        EmployeeCreateDto request);

    Task<EmployeeResponseDto> UpdateAsync(
        int id,
        EmployeeUpdateDto request);

    Task SoftDeleteAsync(int id);
}