using System.Collections.Generic;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Department { get; set; }
        public required string Position { get; set; }
        public required string Status { get; set; }
        public required string ProfileImage { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}