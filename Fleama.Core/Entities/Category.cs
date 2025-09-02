using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class Category : BaseEntity
    {
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        public int? ImageId { get; set; }
        
        [Display(Name = "Görseller")]
        public Image Image { get; set; }

        [Display(Name = "Üst Menüde Gösterilsin mi?")]
        public bool IsTopMenu { get; set; }

        [Display(Name = "Üst Kategori ID")]
        public int? ParentId { get; set; } = 0;

        [Display(Name = "Sıra Numarası")]
        public int OrderNo { get; set; }

        [Display(Name = "Ürünler")]
        public IList<Product>? Products { get; set; }
    }
}