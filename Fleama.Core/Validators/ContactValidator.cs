using Fleama.Core.Entities;
using FluentValidation;

namespace Fleama.Core.Validators
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Adı alanı boş geçilemez.")
                .MaximumLength(100).WithMessage("Adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Soyadı alanı boş geçilemez.")
                .MaximumLength(100).WithMessage("Soyadı en fazla 100 karakter olabilir.");

            RuleFor(x => x.UserName)
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir.")
                .When(x => !string.IsNullOrEmpty(x.UserName));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş geçilemez.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(200).WithMessage("E-posta en fazla 200 karakter olabilir.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon alanı boş geçilemez.")
                .MaximumLength(20).WithMessage("Telefon en fazla 20 karakter olabilir.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Mesaj alanı boş geçilemez.")
                .MinimumLength(10).WithMessage("Mesaj en az 10 karakter olmalıdır.")
                .MaximumLength(2000).WithMessage("Mesaj en fazla 2000 karakter olabilir.");
        }
    }
}