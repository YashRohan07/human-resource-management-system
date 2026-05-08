using HRMS.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Data;

// Adds default users when the database is empty
public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.AppUsers.AnyAsync())
        {
            return;
        }

        var users = new List<AppUser>
        {
            new()
            {
                FullName = "System Admin",
                Email = "admin@hrms.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin"
            },
            new()
            {
                FullName = "HR User",
                Email = "hr@hrms.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Hr123!"),
                Role = "HR"
            }
        };

        await context.AppUsers.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}