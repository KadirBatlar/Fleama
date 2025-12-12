using Fleama.Core.Entities;
using FluentValidation;

namespace Fleama.Core.Validators
{
    public class BrandValidator : AbstractValidator<Brand>
    {
        public BrandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Marka adı alanı boş geçilemez.")
                .MaximumLength(100).WithMessage("Marka adı en fazla 100 karakter olabilir.");
        }
    }
}