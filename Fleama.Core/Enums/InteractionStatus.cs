using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Enums
{
    public enum InteractionStatus
    {
        None = 0,
        [Display(Name = "Talep Edildi")]
        Requested = 1,

        [Display(Name = "Kabul Edildi")]
        Accepted = 2,

        [Display(Name = "Tamamlandı")]
        Completed = 3,

        [Display(Name = "İptal Edildi")]
        Cancelled = 4
    }
}
