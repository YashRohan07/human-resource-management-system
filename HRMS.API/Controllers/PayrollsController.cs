using HRMS.API.Common;
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
}