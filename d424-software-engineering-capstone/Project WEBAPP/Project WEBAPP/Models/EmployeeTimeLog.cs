public class EmployeeTimeLog
{
    public int Id { get; set; }
    public string EmployeeId { get; set; }
    public DateTime ClockInTime { get; set; }
    public DateTime? ClockOutTime { get; set; }

    // Navigation property to Employee
    public virtual Employee Employee { get; set; }
}

public class Employee
{
    public string EmployeeId { get; set; }
    public string Name { get; set; }

    // Collection of time logs for this employee
    public virtual ICollection<EmployeeTimeLog> TimeLogs { get; set; }
}
