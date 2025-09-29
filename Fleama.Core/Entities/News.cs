using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class News : BaseEntity
    {
        [Display(Name = "Haber Başlığı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        public int? ImageId { get; set; }

        [Display(Name = "Görsel")]
        public Image? Image { get; set; }
    }
}