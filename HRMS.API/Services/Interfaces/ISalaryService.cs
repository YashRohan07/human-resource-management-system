using HRMS.API.DTOs.Salaries;

namespace HRMS.API.Services.Interfaces;

// Business operations for salaries
public interface ISalaryService
{
    Task<SalaryResponseDto> GetByEmployeeIdAsync(int employeeId);

    Task<SalaryResponseDto> CreateAsync(SalaryCreateDto request);

    Task<SalaryResponseDto> UpdateAsync(int id, SalaryUpdateDto request);
}