using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Fleama.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Fleama.Core.Enums;

namespace Fleama.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly INewsService _newsService;
        private readonly IBaseService<Contact> _contactService;

        public HomeController(IProductService productService, INewsService newsService, IBaseService<Contact> contactService)
        {
            _productService = productService;
            _newsService = newsService;
            _contactService = contactService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel()
            {
                Products = await _productService.GetAllAsync(p => p.IsActive && p.ApproveStatus == ProductApproveStatus.Approved), 
                News = await _newsService.GetAllAsync(n => n.IsActive)
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(Contact contact)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _contactService.AddAsync(contact);
                    var result = await _contactService.SaveChangesAsync();
                    if(result > 0)
                    {
                        TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
                                                  <strong>Mesajýnýz Gönderilmiþtir</strong>
                                                  <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
                                                </div>";
                        //await MailHelper.SendMailAsync(contact);
                        return RedirectToAction("ContactUs");
                    }
                    throw new Exception("Mesaj gönderilemedi!");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Hata Oluþtu!");
                }
            }
            return View(contact);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}