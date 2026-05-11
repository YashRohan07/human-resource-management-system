using HRMS.API.Common;
using HRMS.API.Data;
using HRMS.API.Data.QueryModels;
using HRMS.API.DTOs.Payrolls;
using HRMS.API.Entities;
using HRMS.API.Exceptions;
using HRMS.API.Repositories.Interfaces;
using HRMS.API.Services.Calculators;
using HRMS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Services;

// Handles payroll business logic
public class PayrollService : IPayrollService
{
    private readonly IPayrollRepository _payrollRepository;
    private readonly PayrollCalculator _payrollCalculator;
    private readonly ApplicationDbContext _context;

    public PayrollService(
        IPayrollRepository payrollRepository,
        PayrollCalculator payrollCalculator,
        ApplicationDbContext context)
    {
        _payrollRepository = payrollRepository;
        _payrollCalculator = payrollCalculator;
        _context = context;
    }

    public async Task<IEnumerable<PayrollResponseDto>> GenerateAsync(
        PayrollGenerateDto request)
    {
        ValidateFutureMonth(request.Month, request.Year);

        var employees = await _payrollRepository.GetActiveEmployeesAsync();
        var employeeList = employees.ToList();

        if (!employeeList.Any())
        {
            return [];
        }

        var employeeIds = employeeList.Select(x => x.Id).ToList();

        var salaries = await _payrollRepository
            .GetSalariesForEmployeesAsync(employeeIds);

        var salaryMap = salaries.ToDictionary(x => x.EmployeeId);

        var payrolls = new List<Payroll>();

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach (var employee in employeeList)
            {
                if (!salaryMap.TryGetValue(employee.Id, out var salary))
                {
                    continue;
                }

                var exists = await _payrollRepository.ExistsAsync(
                    employee.Id,
                    request.Month,
                    request.Year);

                if (exists)
                {
                    throw new ConflictException(
                        "Payroll already generated for this month.");
                }

                var grossSalary = _payrollCalculator.CalculateGrossSalary(
                    salary.BasicSalary,
                    salary.Bonus);

                var tax = _payrollCalculator.CalculateTax(grossSalary);

                var netSalary = _payrollCalculator.CalculateNetSalary(
                    grossSalary,
                    tax,
                    salary.Deduction);

                var payroll = new Payroll
                {
                    EmployeeId = employee.Id,
                    Month = request.Month,
                    Year = request.Year,
                    BasicSalarySnapshot = salary.BasicSalary,
                    BonusSnapshot = salary.Bonus,
                    DeductionSnapshot = salary.Deduction,
                    Tax = tax,
                    GrossSalary = grossSalary,
                    NetSalary = netSalary,
                    GeneratedAt = DateTime.UtcNow,
                    Employee = employee
                };

                payrolls.Add(payroll);
            }

            if (!payrolls.Any())
            {
                await transaction.CommitAsync();

                return [];
            }

            await _payrollRepository.AddRangeAsync(payrolls);

            await transaction.CommitAsync();

            return payrolls.Select(MapToResponseDto);
        }
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync();

            throw new ConflictException(
                "Payroll already generated for this month.");
        }
        catch
        {
            await transaction.RollbackAsync();

            throw;
        }
    }

    public async Task<PagedResult<PayrollResponseDto>> GetAllAsync(
        PayrollQueryDto query)
    {
        var result = await _payrollRepository.GetAllAsync(query);

        var items = result.Payrolls.Select(MapToResponseDto);

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var totalPages = (int)Math.Ceiling(
            result.TotalCount / (double)pageSize);

        return new PagedResult<PayrollResponseDto>
        {
            Items = items,
            Meta = new PaginationMeta
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = result.TotalCount,
                TotalPages = totalPages
            }
        };
    }

    public async Task<PayrollResponseDto> GetByIdAsync(int id)
    {
        var payroll = await _payrollRepository.GetByIdAsync(id);

        if (payroll is null)
        {
            throw new NotFoundException("Payroll not found.");
        }

        return MapToResponseDto(payroll);
    }

    public async Task<IEnumerable<PayrollResponseDto>> GetByEmployeeIdAsync(
        int employeeId)
    {
        var payrolls = await _payrollRepository.GetByEmployeeIdAsync(
            employeeId);

        return payrolls.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<PayrollSummaryResult>> GetSummaryAsync(
        int month,
        int year)
    {
        ValidateMonthAndYear(month, year);

        return await _payrollRepository.GetSummaryAsync(month, year);
    }

    private static void ValidateMonthAndYear(int month, int year)
    {
        if (month < 1 || month > 12)
        {
            throw new AppException(
                "Month must be between 1 and 12.",
                StatusCodes.Status400BadRequest);
        }

        if (year <= 2000)
        {
            throw new AppException(
                "Year must be greater than 2000.",
                StatusCodes.Status400BadRequest);
        }
    }

    private static void ValidateFutureMonth(int month, int year)
    {
        var currentDate = DateTime.UtcNow;

        if (year > currentDate.Year ||
            year == currentDate.Year && month > currentDate.Month)
        {
            throw new AppException(
                "Future month payroll cannot be generated.",
                StatusCodes.Status400BadRequest);
        }
    }

    private static PayrollResponseDto MapToResponseDto(Payroll payroll)
    {
        return new PayrollResponseDto
        {
            Id = payroll.Id,
            EmployeeId = payroll.EmployeeId,
            EmployeeName = payroll.Employee?.FullName ?? string.Empty,
            Department = payroll.Employee?.Department ?? string.Empty,
            Month = payroll.Month,
            Year = payroll.Year,
            BasicSalarySnapshot = payroll.BasicSalarySnapshot,
            BonusSnapshot = payroll.BonusSnapshot,
            DeductionSnapshot = payroll.DeductionSnapshot,
            Tax = payroll.Tax,
            GrossSalary = payroll.GrossSalary,
            NetSalary = payroll.NetSalary,
            GeneratedAt = payroll.GeneratedAt,
            CreatedAt = payroll.CreatedAt,
            UpdatedAt = payroll.UpdatedAt
        };
    }
}