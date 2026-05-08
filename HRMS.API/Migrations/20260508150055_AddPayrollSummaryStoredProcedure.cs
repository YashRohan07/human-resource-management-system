using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPayrollSummaryStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE sp_GetPayrollSummaryByMonth
                    @Month INT,
                    @Year INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        e.Department,
                        COUNT(DISTINCT e.Id) AS TotalEmployees,
                        SUM(p.NetSalary) AS TotalSalary
                    FROM Payrolls p
                    INNER JOIN Employees e
                        ON p.EmployeeId = e.Id
                    WHERE p.Month = @Month
                      AND p.Year = @Year
                      AND e.IsDeleted = 0
                    GROUP BY e.Department;
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP PROCEDURE IF EXISTS sp_GetPayrollSummaryByMonth;
                """);
        }
    }
}