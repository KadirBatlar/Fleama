using Fleama.Core.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class Image : BaseEntity
{
    [DisplayName("Resim URL")]
    [Required(ErrorMessage = "Resim URL alanı zorunludur.")]
    public string Url { get; set; }

    [DisplayName("Ürün Id")]
    public int? ProductId { get; set; }

    [DisplayName("Ürün")]
    public Product Product { get; set; }

    [DisplayName("Kategori Id")]
    public int? CategoryId { get; set; }

    [DisplayName("Kategori")]
    public Category Category { get; set; }
}