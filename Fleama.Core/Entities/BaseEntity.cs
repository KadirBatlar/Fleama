using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }
        [Display(Name ="Aktif?")]
        public bool IsActive { get; set; }
        [Display(Name ="Kayıt Tarihi")]
        public DateTime CreatedDate { get; set; }
    }
}