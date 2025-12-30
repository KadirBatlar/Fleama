using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Enums
{
    public enum ProductState
    {
        None = 0,
        [Display(Name = "Müsait")]
        Available = 1,

        [Display(Name = "İşlemde")]
        InTransaction = 2,

        [Display(Name = "Tamamlandı")]
        Completed = 3,

        [Display(Name = "Arşivlendi")]
        Archived = 4
    }
}