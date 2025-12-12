using Fleama.Core.Entities;
using FluentValidation;

namespace Fleama.Core.Validators
{
    public class AppUserValidator : AbstractValidator<AppUser>
    {
        public AppUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad alanı boş geçilemez.")
                .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Soyad alanı boş geçilemez.")
                .MaximumLength(100).WithMessage("Soyad en fazla 100 karakter olabilir.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı ad alanı boş geçilemez.")
                .MinimumLength(3).WithMessage("Kullanıcı ad en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kullanıcı ad en fazla 50 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş geçilemez.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(200).WithMessage("E-posta en fazla 200 karakter olabilir.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon alanı boş geçilemez.")
                .MaximumLength(20).WithMessage("Telefon en fazla 20 karakter olabilir.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş geçilemez.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir.");
        }
    }
}