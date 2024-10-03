namespace Project_WEBAPP.Reports
{
    public abstract class EmployeeReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmployeeId { get; set; }  

        // Method to be overridden by derived classes
        public abstract string GenerateReport();
    }

}
