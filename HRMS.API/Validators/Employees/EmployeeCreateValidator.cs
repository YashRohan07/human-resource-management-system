using FluentValidation;
using HRMS.API.DTOs.Employees;

namespace HRMS.API.Validators.Employees;

// Validation rules for creating employee
public class EmployeeCreateValidator : AbstractValidator<EmployeeCreateDto>
{
    public EmployeeCreateValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone is required.")
            .MaximumLength(30);

        RuleFor(x => x.Department)
            .NotEmpty()
            .WithMessage("Department is required.");

        RuleFor(x => x.Position)
            .NotEmpty()
            .WithMessage("Position is required.");

        RuleFor(x => x.AccountNumber)
            .NotEmpty()
            .WithMessage("Account number is required.");

        RuleFor(x => x.EmploymentStatus)
            .NotEmpty()
            .WithMessage("Employment status is required.");
    }
}