using HRMS.API.DTOs.Dashboard;

namespace HRMS.API.Services.Interfaces;

// Handles dashboard summary operations
public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
}