using Fleama.Shared.Dtos;
using FluentValidation;

namespace Fleama.Shared.Validators
{
    public class BlogCreateDTOValidator : AbstractValidator<BlogCreateDTO>
    {
        public BlogCreateDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık alanı boş geçilemez.")
                .MaximumLength(500).WithMessage("Başlık en fazla 500 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik alanı boş geçilemez.")
                .MinimumLength(10).WithMessage("İçerik en az 10 karakter olmalıdır.");
        }
    }
}