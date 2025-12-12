using Fleama.Core.Entities;
using FluentValidation;

namespace Fleama.Core.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı alanı boş geçilemez.")
                .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.OrderNo)
                .GreaterThanOrEqualTo(0).WithMessage("Sıra numarası 0 veya daha büyük olmalıdır.");
        }
    }
}