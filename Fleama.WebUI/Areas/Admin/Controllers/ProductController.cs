using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DatabaseContext _context;

        public ProductController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productContext = _context.Products.Include(p => p.Brand).Include(p => p.Category);
            return View(await productContext.ToListAsync());
        }

        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Set<Product>().ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var products = await _context.Set<Product>().FindAsync(id);
            if (products == null)
                return NotFound();

            return Ok(products);
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

            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewBag["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image is not null)
                    product.Image = await FileHelper.FileLoaderAsync(image, "/Img/Products/");

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Kategoriler = new SelectList(_context.Products, "Id", "Name");
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Kategoriler = new SelectList(_context.Products, "Id", "Name");
            return View(product);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category product, IFormFile? image, bool removeImg = false)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (removeImg)
                        product.Image = string.Empty;

                    if (image is not null)
                        product.Image = await FileHelper.FileLoaderAsync(image, "/Img/Products/");

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Kategoriler = new SelectList(_context.Products, "Id", "Name");
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    FileHelper.FileRemover(product.Image, "/Img/Products/");
                }
                _context.Products.Remove(product);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
