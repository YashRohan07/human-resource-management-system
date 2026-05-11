namespace HRMS.API.Services.Calculators;

// Keeps payroll amount calculation in one place
public class PayrollCalculator
{
    private const decimal TaxRate = 0.10m;

    public decimal CalculateGrossSalary(
        decimal basicSalary,
        decimal bonus)
    {
        return basicSalary + bonus;
    }

    public decimal CalculateTax(decimal grossSalary)
    {
        return grossSalary * TaxRate;
    }

    public decimal CalculateNetSalary(
        decimal grossSalary,
        decimal tax,
        decimal deduction)
    {
        return grossSalary - tax - deduction;
    }
}