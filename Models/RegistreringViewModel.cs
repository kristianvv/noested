

using System.ComponentModel.DataAnnotations;

namespace Noested.Models
{
    public class RegistreringViewModel
    {
        public int EmployeeNumber { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        [Required]
        public required string Role { get; set; } // Add this property for selecting role
    }
}


