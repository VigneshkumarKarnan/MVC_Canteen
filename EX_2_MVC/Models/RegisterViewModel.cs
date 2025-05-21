using System.ComponentModel.DataAnnotations;

namespace EX_2_MVC.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Gender { get; set; }

        public string Address { get; set; }

        [Required]
        public string Role { get; set; } // <-- Added this
    }


}
