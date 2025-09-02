using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IBaseService<Category> _service;

        public CategoriesController(IBaseService<Category> service)
        {
            _service = service;
        }

        public async Task<IActionResult> IndexAsync(int? id)
        {

            if (id == null)
                return NotFound();

            var category = await _service.GetQueryable().Include(p => p.Products).FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }
    }
}