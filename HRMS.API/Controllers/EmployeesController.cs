using HRMS.API.Common;
using HRMS.API.DTOs.Employees;
using HRMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/v1/employees")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    // Get employee list with pagination and filters
    [Authorize(Roles = "Admin,HR")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] EmployeeQueryDto query)
    {
        var result = await _employeeService.GetAllAsync(query);

        return Ok(new ApiResponse<PagedResult<EmployeeResponseDto>>
        {
            Success = true,
            Message = "Employees fetched successfully.",
            Data = result
        });
    }

    // Get employee by id
    [Authorize(Roles = "Admin,HR")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _employeeService.GetByIdAsync(id);

        return Ok(new ApiResponse<EmployeeResponseDto>
        {
            Success = true,
            Message = "Employee fetched successfully.",
            Data = result
        });
    }

    // Create new employee
    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<IActionResult> Create(
        EmployeeCreateDto request)
    {
        var result = await _employeeService.CreateAsync(request);

        return StatusCode(StatusCodes.Status201Created,
            new ApiResponse<EmployeeResponseDto>
            {
                Success = true,
                Message = "Employee created successfully.",
                Data = result
            });
    }

    // Update employee
    [Authorize(Roles = "Admin,HR")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        EmployeeUpdateDto request)
    {
        var result = await _employeeService.UpdateAsync(id, request);

        return Ok(new ApiResponse<EmployeeResponseDto>
        {
            Success = true,
            Message = "Employee updated successfully.",
            Data = result
        });
    }

    // Soft delete employee
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeService.SoftDeleteAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Employee deleted successfully."
        });
    }
}