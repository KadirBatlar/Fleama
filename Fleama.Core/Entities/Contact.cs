using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class Contact : BaseEntity
    {
        [Display(Name = "Adı"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez")]
        public string Name { get; set; }

        [Display(Name = "Soyadı"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez")]
        public string Surname { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Display(Name = "E-posta"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez")]
        public string Email { get; set; }

        [Display(Name = "Telefon"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez")]
        public string Phone { get; set; }

        [Display(Name = "Mesaj"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez")]
        public string Message { get; set; }
    }
}