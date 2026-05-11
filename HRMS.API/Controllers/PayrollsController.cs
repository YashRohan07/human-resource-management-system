using HRMS.API.Common;
using HRMS.API.Data.QueryModels;
using HRMS.API.DTOs.Payrolls;
using HRMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/v1/payrolls")]
[Authorize]
public class PayrollsController : ControllerBase
{
    private readonly IPayrollService _payrollService;

    public PayrollsController(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    // Generate monthly payroll for active employees
    [Authorize(Roles = "Admin,HR")]
    [HttpPost("generate")]
    public async Task<IActionResult> Generate(
        PayrollGenerateDto request)
    {
        var result = await _payrollService.GenerateAsync(request);

        return StatusCode(StatusCodes.Status201Created,
            new ApiResponse<IEnumerable<PayrollResponseDto>>
            {
                Success = true,
                Message = "Payroll generated successfully.",
                Data = result
            });
    }

    // Get payroll list with pagination and filters
    [Authorize(Roles = "Admin,HR")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PayrollQueryDto query)
    {
        var result = await _payrollService.GetAllAsync(query);

        return Ok(new ApiResponse<PagedResult<PayrollResponseDto>>
        {
            Success = true,
            Message = "Payrolls fetched successfully.",
            Data = result
        });
    }

    // Get department-wise payroll summary
    [Authorize(Roles = "Admin,HR")]
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] int month,
        [FromQuery] int year)
    {
        var result = await _payrollService.GetSummaryAsync(month, year);

        return Ok(new ApiResponse<IEnumerable<PayrollSummaryResult>>
        {
            Success = true,
            Message = "Payroll summary fetched successfully.",
            Data = result
        });
    }

    // Get payroll history for one employee
    [Authorize(Roles = "Admin,HR")]
    [HttpGet("employee/{employeeId:int}")]
    public async Task<IActionResult> GetByEmployeeId(int employeeId)
    {
        var result = await _payrollService.GetByEmployeeIdAsync(employeeId);

        return Ok(new ApiResponse<IEnumerable<PayrollResponseDto>>
        {
            Success = true,
            Message = "Employee payrolls fetched successfully.",
            Data = result
        });
    }

    // Get payroll by id
    [Authorize(Roles = "Admin,HR")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _payrollService.GetByIdAsync(id);

        return Ok(new ApiResponse<PayrollResponseDto>
        {
            Success = true,
            Message = "Payroll fetched successfully.",
            Data = result
        });
    }
}