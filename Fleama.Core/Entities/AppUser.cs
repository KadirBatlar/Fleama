using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class AppUser : BaseEntity
    {
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Soyadı")]
        public string Surname { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        public string Email { get; set; }
        
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Admin?")]
        public bool IsAdmin { get; set; }
        public Guid? UserGuid { get; set; } = Guid.NewGuid();

        // Navigation properties
        public ICollection<UserFavoriteProduct>? FavoriteProducts { get; set; }
    }
}