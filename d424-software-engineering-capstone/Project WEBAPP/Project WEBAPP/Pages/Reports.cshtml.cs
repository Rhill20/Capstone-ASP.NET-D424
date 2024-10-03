using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_WEBAPP.Data;
using System.Globalization;

namespace Project_WEBAPP.Pages
{
    public class ReportsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReportsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string ReportResult { get; set; }
        public List<Employee> EmployeeList { get; set; }

        
        public IActionResult OnGet()
        {
            // Check if the session contains the login information
            if (HttpContext.Session.GetString("IsAdminLoggedIn") != "true")
            {
                return RedirectToPage("/Login");
            }

            PopulateEmployeeList();
            return Page();
        }

        private void PopulateEmployeeList()
        {
            EmployeeList = _context.Employees.ToList();
        }
        public void OnPostGenerateTotalHoursReport(string startMonth)
        {
            if (string.IsNullOrEmpty(startMonth))
            {
                ReportResult = "Please select a month.";
                PopulateEmployeeList();
                return;
            }

            DateTime reportStartDate = DateTime.ParseExact(startMonth, "yyyy-MM", null);
            DateTime reportEndDate = reportStartDate.AddMonths(1).AddDays(-1);

            var report = new TotalHoursReport(_context)
            {
                StartDate = reportStartDate,
                EndDate = reportEndDate
            };

            ReportResult = report.GenerateReport();
            PopulateEmployeeList();  // Repopulate EmployeeList
        }
        public void OnPostGenerateIndividualMonthlyReport(string employeeId, string startMonth)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                ReportResult = "Please select an employee.";
                PopulateEmployeeList();
                return;
            }

            if (string.IsNullOrEmpty(startMonth))
            {
                ReportResult = "Please select a month.";
                PopulateEmployeeList();
                return;
            }

            DateTime reportStartDate = DateTime.ParseExact(startMonth, "yyyy-MM", null);
            DateTime reportEndDate = reportStartDate.AddMonths(1).AddDays(-1);

            var report = new IndividualMonthlyHoursReport(_context)
            {
                EmployeeId = employeeId,
                StartDate = reportStartDate,
                EndDate = reportEndDate
            };

            ReportResult = report.GenerateReport();
            PopulateEmployeeList();  // Repopulate EmployeeList
        }

        public void OnPostGenerate40HourCheckReport(string startMonth)
        {
            if (string.IsNullOrEmpty(startMonth))
            {
                ReportResult = "Please select a month.";
                PopulateEmployeeList();
                return;
            }

            DateTime reportStartDate = DateTime.ParseExact(startMonth, "yyyy-MM", null);
            DateTime reportEndDate = reportStartDate.AddMonths(1).AddDays(-1);

            var logs = _context.EmployeeTimeLogs
                .Where(log => log.ClockInTime >= reportStartDate && log.ClockInTime <= reportEndDate)
                .Where(log => log.ClockOutTime.HasValue && log.ClockOutTime.Value > log.ClockInTime)
                .ToList();

            var allEmployees = _context.Employees.ToList();
            var employeeGroups = logs.GroupBy(log => log.EmployeeId).ToDictionary(g => g.Key, g => g); 

            string report = "40-Hour Check for All Employees (Weekly):\n\n";

            foreach (var employee in allEmployees)
            {
                if (!employeeGroups.ContainsKey(employee.EmployeeId))
                {
                    // Employee has no logs for the month
                    report += $"Employee {employee.EmployeeId} - {employee.Name}: No hours worked this month.\n\n";
                    continue;
                }

                report += $"Employee {employee.EmployeeId} - {employee.Name}:\n";

                var weeklyLogs = employeeGroups[employee.EmployeeId]
                    .GroupBy(log => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                        log.ClockInTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday));

                // Loop through all weeks
                for (int week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(reportStartDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                     week <= CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(reportEndDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                     week++)
                {
                    var weekLogs = weeklyLogs.FirstOrDefault(w => w.Key == week);

                    if (weekLogs != null)
                    {
                        double totalHours = weekLogs
                            .Where(log => log.ClockOutTime.HasValue)
                            .Sum(log => (log.ClockOutTime.Value - log.ClockInTime).TotalHours);

                        // Check if employee met 40 hours for the week
                        report += $"  Week {week}: {totalHours:F2} hours - " +
                                  (totalHours >= 40 ? "Met 40 hours" : "Did not meet 40 hours") + "\n";
                    }
                    else
                    {
                        report += $"  Week {week}: No hours worked\n";
                    }
                }

                report += "\n";
            }

            ReportResult = report;
            PopulateEmployeeList();  // Repopulate EmployeeList
        }

    }
}


