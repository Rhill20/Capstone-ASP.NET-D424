using System.ComponentModel.DataAnnotations;

namespace Project_WEBAPP.Models
{
    public class Employee
    {
        [Key]
        public string EmployeeId { get; set; }
        public string Name { get; set; }


    }
}