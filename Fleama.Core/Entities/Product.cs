using System.ComponentModel.DataAnnotations;
using Fleama.Core.Enums;

namespace Fleama.Core.Entities
{
    public class Product : BaseEntity
    {
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        public IList<int>? ImageId { get; set; }

        [Display(Name = "Görseller")]
        public IList<Image>? Images { get; set; }

        [Display(Name = "Ürün Kodu")]
        public string? ProductCode { get; set; }

        [Display(Name = "Fiyat")]
        public decimal? Price { get; set; }

        [Display(Name = "Durum")]
        public ProductApproveStatus ApproveStatus { get; set; } = ProductApproveStatus.Pending;


        [Display(Name = "Skor")]
        public int Score { get; set; } = 0;

        public int ViewCount { get; set; } = 0;
        public int FavoriteCount { get; set; } = 0;
        public int SwapRequestCount { get; set; } = 0;


        // 🔗 Relations

        [Display(Name = "Kategori ID")]
        public int? CategoryId { get; set; }

        [Display(Name = "Kategori")]
        public Category? Category { get; set; }


        [Display(Name = "Marka ID")]
        public int? BrandId { get; set; }

        [Display(Name = "Marka")]
        public Brand? Brand { get; set; }


        [Display(Name = "Kullanıcı ID")]
        public int? UserId { get; set; }

        [Display(Name = "Kullanıcı")]
        public AppUser? User { get; set; }
    }
}
