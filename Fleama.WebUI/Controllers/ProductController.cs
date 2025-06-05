using Fleama.Data;
using Fleama.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly DatabaseContext _context;

        public ProductController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search = "")
        {
            var productContext = _context.Products
                                 .Where(p => p.IsActive && p.IsHome &&
                                     (p.Name.Contains(search) || p.Description.Contains(search)))
                                 .Include(p => p.Brand)
                                 .Include(p => p.Category);

            return View(await productContext.ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

            var model = new ProductDetailViewModel() { Product = product,
                RelatedProducts = _context.Products.Where(p => p.IsActive && p.CategoryId == product.CategoryId && p.Id != product.Id)};
            return View(model);
        }
    }
}