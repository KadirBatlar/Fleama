using Fleama.Core.Entities;
using FluentValidation;

namespace Fleama.Core.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı alanı boş geçilemez.")
                .MaximumLength(200).WithMessage("Ürün adı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Açıklama en fazla 2000 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.ProductCode)
                .MaximumLength(50).WithMessage("Ürün kodu en fazla 50 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.ProductCode));

            // CategoryId and BrandId are required for admin submissions
            // For user submissions, these are set to null in the controller and validation is bypassed
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kategori seçilmelidir.")
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir.")
                .When(x => x.CategoryId.HasValue);

            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("Marka seçilmelidir.")
                .GreaterThan(0).WithMessage("Geçerli bir marka seçilmelidir.")
                .When(x => x.BrandId.HasValue);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Fiyat 0 veya daha büyük olmalıdır.")
                .When(x => x.Price.HasValue);
        }
    }
}