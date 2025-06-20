using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Fleama.WebUI.Controllers
{
    public class NewsController : Controller
    {

        private readonly IService<News> _service;

        public NewsController(IService<News> service)
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
                return NotFound();

            var news = await _service.GetAsync(x => x.Id == id);
            if (news == null)
                return NotFound();

            return View(news);
        }
    }
}