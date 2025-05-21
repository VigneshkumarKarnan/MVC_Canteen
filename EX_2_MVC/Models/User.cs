using System.ComponentModel.DataAnnotations;

namespace EX_2_MVC.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string Role { get; set; } // <-- Make sure this is here
    }

}
