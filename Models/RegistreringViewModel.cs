

using System.ComponentModel.DataAnnotations;

namespace Innlogging.Models
{
    public class RegistreringViewModel
    {
        public int EmployeeNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; } // Add this property for selecting role
    }
}


