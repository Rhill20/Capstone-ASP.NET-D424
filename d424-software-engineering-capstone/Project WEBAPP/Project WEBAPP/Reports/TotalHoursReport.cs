using Project_WEBAPP.Data;
using Project_WEBAPP.Reports;

public class TotalHoursReport : EmployeeReport
{
    private readonly ApplicationDbContext _context;

    public TotalHoursReport(ApplicationDbContext context)
    {
        _context = context;
    }

    public override string GenerateReport()
    {
        var logs = _context.EmployeeTimeLogs
            .Where(log => log.ClockInTime >= StartDate && log.ClockInTime <= EndDate)
            .Where(log => log.ClockOutTime.HasValue && log.ClockOutTime.Value > log.ClockInTime)
            .ToList();

        var employeeGroups = logs.GroupBy(log => log.EmployeeId);
        string report = "Total hours worked by all employees:\n";

        foreach (var group in employeeGroups)
        {
            double totalHours = group
                .Sum(log => (log.ClockOutTime.Value - log.ClockInTime).TotalHours);

            report += $"Employee {group.Key}: {totalHours:F2} hours\n";
        }

        return report;
    }
}
