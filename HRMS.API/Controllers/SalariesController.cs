using HRMS.API.Common;
using HRMS.API.DTOs.Salaries;
using HRMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/v1/salaries")]
[Authorize]
public class SalariesController : ControllerBase
{
    private readonly ISalaryService _salaryService;

    public SalariesController(ISalaryService salaryService)
    {
        _salaryService = salaryService;
    }

    // Get salary by employee id
    [Authorize(Roles = "Admin,HR")]
    [HttpGet("employee/{employeeId:int}")]
    public async Task<IActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _salaryService.GetByEmployeeIdAsync(employeeId);

        return Ok(new ApiResponse<SalaryResponseDto>
        {
            Success = true,
            Message = "Salary fetched successfully.",
            Data = result
        });
    }

    // Create salary for employee
    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<IActionResult> Create(
        SalaryCreateDto request)
    {
        var result = await _salaryService.CreateAsync(request);

        return StatusCode(StatusCodes.Status201Created,
            new ApiResponse<SalaryResponseDto>
            {
                Success = true,
                Message = "Salary created successfully.",
                Data = result
            });
    }

    // Update existing salary
    [Authorize(Roles = "Admin,HR")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        SalaryUpdateDto request)
    {
        var result = await _salaryService.UpdateAsync(id, request);

        return Ok(new ApiResponse<SalaryResponseDto>
        {
            Success = true,
            Message = "Salary updated successfully.",
            Data = result
        });
    }
}