using Project_WEBAPP.Data;
using Project_WEBAPP.Reports;
using System.Globalization;

public class Employee40HourCheckReport : EmployeeReport
{
    private readonly ApplicationDbContext _context;

    public Employee40HourCheckReport(ApplicationDbContext context)
    {
        _context = context;
    }

    public override string GenerateReport()
    {
        var logs = _context.EmployeeTimeLogs
            .Where(log => log.ClockInTime >= StartDate && log.ClockInTime <= EndDate)
            .ToList();

        var employeeGroups = logs.GroupBy(log => log.EmployeeId);
        string report = "40-hour check report:\n";

        foreach (var group in employeeGroups)
        {
            var weeklyLogs = group
                .GroupBy(log => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(log.ClockInTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday));

            foreach (var week in weeklyLogs)
            {
                double weeklyHours = week
                    .Where(log => log.ClockOutTime.HasValue)
                    .Sum(log => (log.ClockOutTime.Value - log.ClockInTime).TotalHours);

                report += $"Employee {group.Key}, Week {week.Key}: {weeklyHours:F2} hours ({(weeklyHours >= 40 ? "Met" : "Did not meet")} 40-hour requirement)\n";
            }
        }

        return report;
    }
}
