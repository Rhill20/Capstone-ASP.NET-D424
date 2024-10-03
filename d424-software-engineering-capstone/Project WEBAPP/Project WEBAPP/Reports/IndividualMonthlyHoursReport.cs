using Project_WEBAPP.Data;
using Project_WEBAPP.Models;
using Project_WEBAPP.Reports;

public class IndividualMonthlyHoursReport : EmployeeReport
{
    private readonly ApplicationDbContext _context;

    public IndividualMonthlyHoursReport(ApplicationDbContext context)
    {
        _context = context;
    }

    public override string GenerateReport()
    {
        var logs = _context.EmployeeTimeLogs
            .Where(log => log.EmployeeId == EmployeeId && log.ClockInTime >= StartDate && log.ClockInTime <= EndDate)
            .ToList();

        double totalHours = logs
            .Where(log => log.ClockOutTime.HasValue)
        .Sum(log => (log.ClockOutTime.Value - log.ClockInTime).TotalHours);

        return $"Total hours worked by employee {EmployeeId} for the selected month: {totalHours:F2} hours.";
    }
}
