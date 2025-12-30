using System.ComponentModel.DataAnnotations;

namespace Fleama.Core.Enums
{
    public enum InteractionType
    {
        None = 0,
        [Display(Name = "Takas")]
        Swap = 1,

        [Display(Name = "Ödünç")]
        Borrow = 2
    }
}