using System.ComponentModel.DataAnnotations;

namespace Fleama.Shared.Dtos
{
    public class BlogUpdateDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık gereklidir")]
        [Display(Name = "Başlık")]
        [MaxLength(500, ErrorMessage = "Başlık en fazla 500 karakter olabilir")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik gereklidir")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;

        public string? ExistingImageUrl { get; set; }
    }
}

