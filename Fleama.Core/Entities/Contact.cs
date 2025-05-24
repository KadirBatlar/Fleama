using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class Contact : BaseEntity
    {
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Soyadı")]
        public string Surname { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Display(Name = "Mesaj")]
        public string Message { get; set; }
    }
}