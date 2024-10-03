using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_WEBAPP.Data;
using Project_WEBAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_WEBAPP.Pages
{
    public class AdminModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EmployeeTimeLog> TimeLogs { get; set; }
        public IList<Employee> Employees { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchEmployeeId { get; set; }

        [BindProperty]
        public string EmployeeId { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if the session contains the login information
            if (HttpContext.Session.GetString("IsAdminLoggedIn") != "true")
            {
                return RedirectToPage("/Login");
            }

            Employees = await _context.Employees.ToListAsync();
            TimeLogs = await _context.EmployeeTimeLogs.Include(log => log.Employee)
                                                      .OrderByDescending(log => log.ClockInTime)
                                                      .ToListAsync();

            ApplySearchFilters();
            return Page();
        }

        private void ApplySearchFilters()
        {
            if (!string.IsNullOrEmpty(SearchEmployeeId))
            {
                TimeLogs = TimeLogs.Where(tl => tl.EmployeeId.Contains(SearchEmployeeId)).ToList();
            }

            if (!string.IsNullOrEmpty(SearchName))
            {
                TimeLogs = TimeLogs.Where(tl => tl.Employee.Name.Contains(SearchName)).ToList();
            }

            if (DateTime.TryParse(SearchDate, out var date))
            {
                TimeLogs = TimeLogs.Where(tl => tl.ClockInTime.Date == date.Date).ToList();
            }
        }

        public async Task<IActionResult> OnPostAddEmployeeAsync()
        {
            if (!string.IsNullOrWhiteSpace(EmployeeId) && !string.IsNullOrWhiteSpace(Name) && !_context.Employees.Any(e => e.EmployeeId == EmployeeId))
            {
                var employee = new Employee { EmployeeId = EmployeeId, Name = Name };
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToPage();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateLogAsync(int id, DateTime clockInTime, DateTime? clockOutTime)
        {
            var logEntry = await _context.EmployeeTimeLogs.FindAsync(id);
            if (logEntry == null)
            {
                return NotFound();
            }

            // Validation
            if (clockOutTime.HasValue && clockOutTime.Value < clockInTime)
            {
                ModelState.AddModelError(string.Empty, "Clock-out time cannot be before the clock-in time.");

                TimeLogs = await _context.EmployeeTimeLogs.Include(log => log.Employee).ToListAsync();
                Employees = await _context.Employees.ToListAsync();

                return Page();
            }

            logEntry.ClockInTime = clockInTime;
            logEntry.ClockOutTime = clockOutTime;

            try
            {
                // Save 
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the log.");

                // Reload 
                TimeLogs = await _context.EmployeeTimeLogs.Include(log => log.Employee).ToListAsync();
                Employees = await _context.Employees.ToListAsync();
                return Page();
            }

            // If everything is valid and saved, redirect back to the page
            return RedirectToPage();
        }


    }
}

