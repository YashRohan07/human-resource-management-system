using HRMS.API.DTOs.Salaries;
using HRMS.API.Entities;
using HRMS.API.Exceptions;
using HRMS.API.Repositories.Interfaces;
using HRMS.API.Services.Interfaces;

namespace HRMS.API.Services;

// Handles salary business logic
public class SalaryService : ISalaryService
{
    private readonly ISalaryRepository _salaryRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public SalaryService(
        ISalaryRepository salaryRepository,
        IEmployeeRepository employeeRepository)
    {
        _salaryRepository = salaryRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<SalaryResponseDto> GetByEmployeeIdAsync(int employeeId)
    {
        var salary = await _salaryRepository.GetByEmployeeIdAsync(employeeId);

        if (salary is null)
        {
            throw new NotFoundException("Salary not found.");
        }

        return MapToResponseDto(salary);
    }

    public async Task<SalaryResponseDto> CreateAsync(SalaryCreateDto request)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);

        if (employee is null)
        {
            throw new NotFoundException("Employee not found.");
        }

        var salaryExists = await _salaryRepository
            .ExistsByEmployeeIdAsync(request.EmployeeId);

        if (salaryExists)
        {
            throw new ConflictException("Salary already exists for this employee.");
        }

        var salary = new Salary
        {
            EmployeeId = request.EmployeeId,
            BasicSalary = request.BasicSalary,
            Bonus = request.Bonus,
            Deduction = request.Deduction,
            EffectiveFrom = request.EffectiveFrom
        };

        var createdSalary = await _salaryRepository.CreateAsync(salary);

        return MapToResponseDto(createdSalary);
    }

    public async Task<SalaryResponseDto> UpdateAsync(
        int id,
        SalaryUpdateDto request)
    {
        var salary = await _salaryRepository.GetByIdAsync(id);

        if (salary is null)
        {
            throw new NotFoundException("Salary not found.");
        }

        salary.BasicSalary = request.BasicSalary;
        salary.Bonus = request.Bonus;
        salary.Deduction = request.Deduction;
        salary.EffectiveFrom = request.EffectiveFrom;

        await _salaryRepository.UpdateAsync(salary);

        return MapToResponseDto(salary);
    }

    private static SalaryResponseDto MapToResponseDto(Salary salary)
    {
        return new SalaryResponseDto
        {
            Id = salary.Id,
            EmployeeId = salary.EmployeeId,
            BasicSalary = salary.BasicSalary,
            Bonus = salary.Bonus,
            Deduction = salary.Deduction,
            EffectiveFrom = salary.EffectiveFrom,
            CreatedAt = salary.CreatedAt,
            UpdatedAt = salary.UpdatedAt
        };
    }
}