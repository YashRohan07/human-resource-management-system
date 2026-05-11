namespace HRMS.API.DTOs.Payrolls;

// Request payload for payroll generation
public class PayrollGenerateDto
{
    public int Month { get; set; }

    public int Year { get; set; }
}