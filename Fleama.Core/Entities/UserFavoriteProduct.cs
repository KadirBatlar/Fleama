using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class UserFavoriteProduct : BaseEntity
    {
        [Display(Name = "Kullanıcı ID")]
        public int UserId { get; set; }

        [Display(Name = "Kullanıcı")]
        public AppUser User { get; set; }

        [Display(Name = "Ürün ID")]
        public int ProductId { get; set; }

        [Display(Name = "Ürün")]
        public Product Product { get; set; }
    }
}
