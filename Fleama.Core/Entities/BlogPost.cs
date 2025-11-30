using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class BlogPost : BaseEntity
    {
        [Display(Name = "Kullanıcı ID")]
        public int UserId { get; set; }

        [Display(Name = "Kullanıcı")]
        public AppUser? User { get; set; }

        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Display(Name = "İçerik")]
        public string Content { get; set; }

        [Display(Name = "Görsel URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Doğrulanmış")]
        public bool IsVerified { get; set; }

        [Display(Name = "Güncelleme Tarihi")]
        public DateTime? UpdatedAt { get; set; }
    }
}

