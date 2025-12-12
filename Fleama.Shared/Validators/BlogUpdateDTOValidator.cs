using Fleama.Shared.Dtos;
using FluentValidation;

namespace Fleama.Shared.Validators
{
    public class BlogUpdateDTOValidator : AbstractValidator<BlogUpdateDTO>
    {
        public BlogUpdateDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık alanı boş geçilemez.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik alanı boş geçilemez.")
                .MinimumLength(10).WithMessage("İçerik en az 10 karakter olmalıdır.");
        }
    }
}