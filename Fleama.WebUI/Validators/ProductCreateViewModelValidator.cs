using Fleama.WebUI.Areas.Admin.Models;
using FluentValidation;

namespace Fleama.WebUI.Validators
{
    public class ProductCreateViewModelValidator : AbstractValidator<ProductCreateViewModel>
    {
        public ProductCreateViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı alanı boş geçilemez.")
                .MaximumLength(200).WithMessage("Ürün adı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(5000).WithMessage("Açıklama en fazla 5000 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Kategori seçilmelidir.");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("Marka seçilmelidir.");

            RuleFor(x => x.Images)
                .NotNull().WithMessage("Görseller alanı boş olamaz.")
                .Must(images => images != null && images.Count > 0)
                .WithMessage("En az bir görsel seçilmelidir.");
        }
    }
}