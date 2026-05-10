using FluentValidation;
using HRMS.API.DTOs.Salaries;

namespace HRMS.API.Validators.Salaries;

// Validation rules for updating salary
public class SalaryUpdateValidator : AbstractValidator<SalaryUpdateDto>
{
    public SalaryUpdateValidator()
    {
        // Basic salary must be positive
        RuleFor(x => x.BasicSalary)
            .GreaterThan(0)
            .WithMessage("Basic salary must be greater than 0.");

        RuleFor(x => x.Bonus)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Bonus cannot be negative.");

        RuleFor(x => x.Deduction)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Deduction cannot be negative.");

        RuleFor(x => x.EffectiveFrom)
            .NotEmpty()
            .WithMessage("Effective date is required.");
    }
}