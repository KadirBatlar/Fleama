using System.ComponentModel.DataAnnotations;

namespace Fleama.WebUI.Models
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress), Required(ErrorMessage = "Email Alanı Doldurulmalıdır!")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [DataType(DataType.Password), Required(ErrorMessage = "Şifre Alanı Doldurulmalıdır!")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}