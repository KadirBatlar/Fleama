using Fleama.Shared.Dtos;
using FluentValidation;

namespace Fleama.Shared.Validators
{
    public class FileDtoValidator : AbstractValidator<FileDto>
    {
        public FileDtoValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("Dosya adı alanı boş geçilemez.")
                .MaximumLength(255).WithMessage("Dosya adı en fazla 255 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotNull().WithMessage("Dosya içeriği boş olamaz.")
                .Must(content => content != null && content.Length > 0)
                .WithMessage("Dosya içeriği boş olamaz.");
        }
    }
}