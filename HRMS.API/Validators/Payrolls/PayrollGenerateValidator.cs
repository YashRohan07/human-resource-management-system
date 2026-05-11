using FluentValidation;
using HRMS.API.DTOs.Payrolls;

namespace HRMS.API.Validators.Payrolls;

// Validation rules for payroll generation
public class PayrollGenerateValidator : AbstractValidator<PayrollGenerateDto>
{
    public PayrollGenerateValidator()
    {
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12)
            .WithMessage("Month must be between 1 and 12.");

        RuleFor(x => x.Year)
            .GreaterThan(2000)
            .WithMessage("Year must be greater than 2000.");
    }
}