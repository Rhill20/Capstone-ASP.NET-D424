using Microsoft.EntityFrameworkCore;
using Project_WEBAPP.Models;

namespace Project_WEBAPP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeTimeLog> EmployeeTimeLogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; } 
    }
}
