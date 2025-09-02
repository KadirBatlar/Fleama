using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Fleama.WebUI.Models;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Authentication; //Login
using Microsoft.AspNetCore.Authorization; //Login
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; //Login

namespace Fleama.WebUI.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly IBaseService<AppUser> _appUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IBaseService<AppUser> appUserService, IHttpContextAccessor httpContextAccessor)
        {
            _appUserService = appUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        public async Task<IActionResult> IndexAsync()        
        {
            AppUser user = await _appUserService.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if(user is null)
            {
                return NotFound();
            }
            var model = new UserEditViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                Phone = user.Phone,
                Surname = user.Surname,
            };
            return View(model);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> IndexAsync(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUser user = await _appUserService.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);

                    if (user is not null)
                    {
                        user.UserName = model.UserName;
                        user.Surname = model.Surname;
                        user.Phone = model.Phone;
                        user.Name = model.Name;
                        user.Password = model.Password;
                        user.Email = model.Email;

                        _appUserService.Update(user);
                        var result = _appUserService.SaveChanges();

                        if (result > 0)
                        {
                            TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
                                                  <strong>İşlem Başarılı!</strong>
                                                  <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
                                                </div>";
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(model);
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignInAsync(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = await _appUserService.GetAsync(x => x.Email == loginViewModel.Email & x.Password == loginViewModel.Password & x.IsActive);
                    if (account == null)
                    {
                        ModelState.AddModelError("", "Giriş Başarısız!");
                    }
                    else
                    {
                        var claims = new List<Claim>()
                        {
                            new(ClaimTypes.Name, account.Name),
                            new(ClaimTypes.Role, account.IsAdmin ? "Admin" : "User"),
                            new(ClaimTypes.Email, account.Email),
                            new("UserId", account.Id.ToString()),
                            new("UserGuid", account.UserGuid.ToString()),                            
                        };
                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.SignInAsync(userPrincipal);
                        return Redirect(string.IsNullOrEmpty(loginViewModel.ReturnUrl) ? "/" : loginViewModel.ReturnUrl);
                    }
                }
                catch (Exception error)
                {
                    //log
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUpAsync(AppUser appUser)
        {
            appUser.IsActive = true;
            appUser.IsAdmin = false;
            if(ModelState.IsValid)
            {
                await _appUserService.AddAsync(appUser);
                await _appUserService.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(appUser);
        }

        public IActionResult PasswordRenew()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordRenewAsync(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Email alanı boş geçilemez!");
                return View();
            }
            AppUser user = await _appUserService.GetAsync(x => x.Email == email);
            if (user is null)
            {
                ModelState.AddModelError("", "Girdiğiniz Email Bulunamadı!");
                return View();
            }
            var request = _httpContextAccessor.HttpContext.Request;
            string domain = $"{request.Scheme}://{request.Host}";

            string message = $"Sayın {user.Name} {user.Surname} Şifrenizi Yenilemek İçin Lütfen <a href='{domain}/Account/PasswordRenew?user={user.UserGuid.ToString()}'>Buraya Tıklayınız!</a>";
            //mail server'ı oluşturulduğunda test edilecek!
            var result = await MailHelper.SendMailAsync(email, "Şifremi Yenile", message);
            if (result)
            {
                TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
        <strong>Şifre Sıfırlama Bağlantınız Mail Adresinize Gönderilmiştir!</strong>
        <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
    </div>";
            }
            else
            {
                TempData["Message"] = @"<div class=""alert alert-danger alert-dismissible fade show"" role=""alert"">
        <strong>Şifre Sıfırlama Bağlantınız Mail Adresinize Gönderilemedi!</strong>
        <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
    </div>";
            }

            return View();
        }

        public async Task<IActionResult> PasswordChangeAsync(string user)
        {
            if (user is null)
            {
                return BadRequest("Geçersiz İstek!");
            }

            AppUser appUser = await _appUserService.GetAsync(x => x.UserGuid.ToString() == user);
            if (appUser is null)
            {
                return NotFound("Geçersiz İstek!");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(string userGuid, string Password)
        {
            if (userGuid is null)
            {
                return BadRequest("Geçersiz İstek!");
            }

            AppUser user = await _appUserService.GetAsync(x => x.UserGuid.ToString() == userGuid);
            if (user is null)
            {
                ModelState.AddModelError("","Geçersiz Değer!");
                return View();
            }
            user.Password = Password;
            var result = await _appUserService.SaveChangesAsync();
            if (result > 0)
            {
                TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
        <strong>Şifreniz Güncellenmiştir! Giriş Ekranından Oturum Açabilirsiniz.</strong>
        <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
    </div>";
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme Başarısız!");
            }
                return View();
        }
    }
}