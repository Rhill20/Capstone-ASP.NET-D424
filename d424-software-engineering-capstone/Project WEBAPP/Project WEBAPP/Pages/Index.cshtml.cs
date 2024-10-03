using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_WEBAPP.Data;
using Project_WEBAPP.Models;
using System;
using System.Linq;

namespace Project_WEBAPP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string EmployeeId { get; set; }

        public bool EmployeeExists { get; set; }

        public EmployeeTimeLog? RecentLog { get; set; }
        public string? Message { get; set; }

        public void OnPost()
        {
            if (string.IsNullOrWhiteSpace(EmployeeId))
            {
                Message = "Employee ID is required.";
                EmployeeExists = false;
                return;
            }

            EmployeeExists = _context.Employees.Any(e => e.EmployeeId == EmployeeId);

            if (!EmployeeExists)
            {
                Message = "Not a valid employee number.";
                return;
            }

            // Retrieve the most recent log for employee
            RecentLog = _context.EmployeeTimeLogs
                                .Where(log => log.EmployeeId == EmployeeId)
                                .OrderByDescending(t => t.ClockInTime)
                                .FirstOrDefault();
        }
    

    public IActionResult OnPostClockIn()
        {
            if (RecentLog == null || RecentLog.ClockOutTime.HasValue)
            {
                var newLog = new EmployeeTimeLog
                {
                    EmployeeId = EmployeeId,
                    ClockInTime = DateTime.Now
                };

                _context.EmployeeTimeLogs.Add(newLog);
                _context.SaveChanges();
                Message = "Clocked In Successfully";
            }
            else
            {
                Message = "Already clocked in. Please clock out before clocking in again.";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostClockOut()
        {
            if (string.IsNullOrWhiteSpace(EmployeeId))
            {
                Message = "Employee ID is required.";
                return Page();
            }
            RecentLog = _context.EmployeeTimeLogs
                                .Where(log => log.EmployeeId == EmployeeId)
                                .OrderByDescending(log => log.ClockInTime)
                                .FirstOrDefault();

            if (RecentLog != null && !RecentLog.ClockOutTime.HasValue)
            {

                RecentLog.ClockOutTime = DateTime.Now;
                _context.SaveChanges(); //saved to the database
                Message = "Clocked Out Successfully";
            }
            else
            {
                Message = "Cannot clock out without clocking in.";
            }

            return RedirectToPage();
        }
    }
}