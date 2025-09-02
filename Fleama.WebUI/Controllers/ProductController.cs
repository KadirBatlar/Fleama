using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Fleama.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IBaseService<Product> _service;

        public ProductController(IBaseService<Product> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string search = "")
        {
            var productContext = _service.GetAllAsync(p => p.IsActive && p.IsHome &&
                                     (p.Name.Contains(search) || p.Description.Contains(search)));

            /*var productContext = _context.Products
                                 .Where(p => p.IsActive && p.IsHome &&
                                     (p.Name.Contains(search) || p.Description.Contains(search)))
                                 .Include(p => p.Brand)
                                 .Include(p => p.Category);*/

            return View(await productContext);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _service.GetQueryable()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

            var model = new ProductDetailViewModel() { Product = product,
                RelatedProducts = _service.GetQueryable().Where(p => p.IsActive && p.CategoryId == product.CategoryId && p.Id != product.Id)};
            return View(model);
        }
    }
}