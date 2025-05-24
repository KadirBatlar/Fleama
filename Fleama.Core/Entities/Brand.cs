using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Entities
{
    public class Brand : BaseEntity
    {
        [Display(Name = "Marka Adı")]
        public string Name { get; set; }
    }
}