using System.ComponentModel.DataAnnotations;

namespace Dreamland_Suit_API.Models.Account.SignUp
{
    public class UserRegister
    {
        [Required(ErrorMessage="User Name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
