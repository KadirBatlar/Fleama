using System.ComponentModel.DataAnnotations;
using Fleama.Core.Enums;

namespace Fleama.Core.Entities
{
    public class ProductInteraction : BaseEntity
    {
        [Display(Name = "Etkileşim Türü")]
        public InteractionType InteractionType { get; set; }

        [Display(Name = "Durum")]
        public InteractionStatus Status { get; set; }

        [Display(Name = "Talep Eden Kullanıcı ID")]
        public int RequesterUserId { get; set; }

        [Display(Name = "Talep Eden Kullanıcı")]
        public AppUser? RequesterUser { get; set; }

        [Display(Name = "Ürün Sahibi Kullanıcı ID")]
        public int OwnerUserId { get; set; }

        [Display(Name = "Ürün Sahibi Kullanıcı")]
        public AppUser? OwnerUser { get; set; }

        [Display(Name = "Ürün ID")]
        public int ProductId { get; set; }

        [Display(Name = "Ürün")]
        public Product? Product { get; set; }

        [Display(Name = "Tamamlanma Tarihi")]
        public DateTime? CompletedAt { get; set; }
    }
}
