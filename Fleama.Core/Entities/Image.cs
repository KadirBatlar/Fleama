using Fleama.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class Image : BaseEntity
    {
        [DisplayName("Resim URL")]
        [Required(ErrorMessage = "Resim URL alanı zorunludur.")]
        public string Url { get; set; }

        [DisplayName("Referans Id")]
        [Required]
        public int ReferenceId { get; set; }

        [DisplayName("Görsel Tipi")]
        [Required]
        public ImageType ImageType { get; set; }
    }
}