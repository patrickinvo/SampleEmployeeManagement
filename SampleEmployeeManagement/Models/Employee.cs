namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Department { get; set; }
        public required string Position { get; set; }
        public EmployeeStatus Status { get; set; }
        public required string ProfileImage { get; set; }
    }   

    public enum EmployeeStatus
    {
        ACTIVE,
        INACTIVE,
        RESIGN,
        TERMINATED,
        ON_LEAVE,
        RETIRED,
        SUSPENDED
    }
}