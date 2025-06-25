using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Fleama.WebUI.Controllers
{
    public class NewsController : Controller
    {

        private readonly IBaseService<News> _service;

        public NewsController(IBaseService<News> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound("Geçersiz İstek!");

            var news = await _service.GetAsync(x => x.Id == id && x.IsActive);
            if (news == null)
                return NotFound("Geçerli Bir Kampanya Bulunamadı.");

            return View(news);
        }
    }
}