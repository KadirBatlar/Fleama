using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class Product : BaseEntity
    {
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Görsel")]
        public string? Image { get; set; }

        [Display(Name = "Ürün Kodu")]
        public string? ProductCode { get; set; }

        [Display(Name = "Anasayfada Gösterilsin mi?")]
        public bool IsHome { get; set; }

        [Display(Name = "Kategori ID")]
        public int CategoryId { get; set; }

        [Display(Name = "Kategori")]
        public Category? Category { get; set; }

        [Display(Name = "Marka ID")]
        public int BrandId { get; set; }

        [Display(Name = "Marka")]
        public Brand? Brand { get; set; }

        [Display(Name = "Sıra Numarası")]
        public int OrderNo { get; set; }
    }
}