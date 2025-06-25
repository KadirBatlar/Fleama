using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class MainController : Controller
    {
        private readonly IBaseService<AppUser> _appUserService;
        private readonly IBaseService<Category> _categoryService;
        private readonly IBaseService<News> _newsService;
        public MainController(IBaseService<AppUser> appUserService, IBaseService<Category> categoryService, IBaseService<News> newsService)
        {
            _appUserService = appUserService;
            _categoryService = categoryService;
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalUsers = await _appUserService.GetTotalCountAsync();
            ViewBag.TotalCategories = await _categoryService.GetTotalCountAsync();
            ViewBag.TotalNews = await _newsService.GetTotalCountAsync();

            return View();
        }
    }
}