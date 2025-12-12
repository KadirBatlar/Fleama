using Fleama.Core.Entities;
using FluentValidation;

namespace Fleama.Core.Validators
{
    public class NewsValidator : AbstractValidator<News>
    {
        public NewsValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Haber başlığı alanı boş geçilemez.")
                .MaximumLength(200).WithMessage("Haber başlığı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}