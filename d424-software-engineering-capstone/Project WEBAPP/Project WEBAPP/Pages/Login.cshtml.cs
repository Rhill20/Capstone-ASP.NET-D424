using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_WEBAPP.Data;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Project_WEBAPP.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var adminUser = await _context.AdminUsers
                                          .FirstOrDefaultAsync(u => u.Username == Username);

            if (adminUser != null && BCrypt.Net.BCrypt.Verify(Password, adminUser.PasswordHash))
            {
                HttpContext.Session.SetString("IsAdminLoggedIn", "true");
                return RedirectToPage("/86247913prot");  // Redirect to the Admin page after login
            }

            Message = "Invalid login attempt.";
            return Page();
        }
    }
}
