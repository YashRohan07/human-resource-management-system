using HRMS.API.Common;
using HRMS.API.DTOs.Employees;
using HRMS.API.Entities;
using HRMS.API.Exceptions;
using HRMS.API.Repositories.Interfaces;
using HRMS.API.Services.Interfaces;

namespace HRMS.API.Services;

// Handles employee business logic
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<PagedResult<EmployeeResponseDto>> GetAllAsync(
        EmployeeQueryDto query)
    {
        var result = await _employeeRepository.GetAllAsync(query);

        var items = result.Employees.Select(MapToResponseDto);

        var totalPages = (int)Math.Ceiling(
            result.TotalCount / (double)query.PageSize);

        return new PagedResult<EmployeeResponseDto>
        {
            Items = items,
            Meta = new PaginationMeta
            {
                CurrentPage = query.Page,
                PageSize = query.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = totalPages
            }
        };
    }

    public async Task<EmployeeResponseDto> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee is null)
        {
            throw new NotFoundException("Employee not found.");
        }

        return MapToResponseDto(employee);
    }

    public async Task<EmployeeResponseDto> CreateAsync(
        EmployeeCreateDto request)
    {
        var emailExists = await _employeeRepository
            .ExistsByEmailAsync(request.Email);

        if (emailExists)
        {
            throw new ConflictException("Email already exists.");
        }

        var employee = new Employee
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Department = request.Department,
            Position = request.Position,
            AccountNumber = request.AccountNumber,
            EmploymentStatus = request.EmploymentStatus
        };

        var createdEmployee = await _employeeRepository
            .CreateAsync(employee);

        return MapToResponseDto(createdEmployee);
    }

    public async Task<EmployeeResponseDto> UpdateAsync(
        int id,
        EmployeeUpdateDto request)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee is null)
        {
            throw new NotFoundException("Employee not found.");
        }

        var emailExists = await _employeeRepository
            .ExistsByEmailAsync(request.Email, id);

        if (emailExists)
        {
            throw new ConflictException("Email already exists.");
        }

        employee.FullName = request.FullName;
        employee.Email = request.Email;
        employee.Phone = request.Phone;
        employee.Department = request.Department;
        employee.Position = request.Position;
        employee.AccountNumber = request.AccountNumber;
        employee.EmploymentStatus = request.EmploymentStatus;

        await _employeeRepository.UpdateAsync(employee);

        return MapToResponseDto(employee);
    }

    public async Task SoftDeleteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee is null)
        {
            throw new NotFoundException("Employee not found.");
        }

        await _employeeRepository.SoftDeleteAsync(employee);
    }

    private static EmployeeResponseDto MapToResponseDto(
        Employee employee)
    {
        return new EmployeeResponseDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Email = employee.Email,
            Phone = employee.Phone,
            Department = employee.Department,
            Position = employee.Position,
            AccountNumber = employee.AccountNumber,
            EmploymentStatus = employee.EmploymentStatus,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }
}