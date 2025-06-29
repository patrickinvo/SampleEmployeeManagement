using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeCreateEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public required string Department { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public required string Position { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public required string Status { get; set; }

        public string? ProfileImage { get; set; }
    }
}