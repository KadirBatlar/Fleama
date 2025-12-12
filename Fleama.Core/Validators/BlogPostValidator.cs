using Fleama.Core.Entities;
using FluentValidation;

namespace Fleama.Core.Validators
{
    public class BlogPostValidator : AbstractValidator<BlogPost>
    {
        public BlogPostValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Kullanıcı ID geçerli bir değer olmalıdır.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık alanı boş geçilemez.")
                .MaximumLength(500).WithMessage("Başlık en fazla 500 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik alanı boş geçilemez.")
                .MinimumLength(10).WithMessage("İçerik en az 10 karakter olmalıdır.");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500).WithMessage("Görsel URL en fazla 500 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.ImageUrl));
        }
    }
}