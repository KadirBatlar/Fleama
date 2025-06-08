using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DatabaseContext _context;

        public CategoryController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            // Üst kategori adı gösterimi
            if (category.ParentId != null)
            {
                var parent = await _context.Categories.FindAsync(category.ParentId);
                ViewBag.ParentName = parent?.Name ?? "-";
            }
            else
            {
                ViewBag.ParentName = "-";
            }

            return View(category);
        }

        public IActionResult Create()
        {
            ViewBag.Kategoriler = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                    category.Image = await FileHelper.FileLoaderAsync(image, "/Img/Categories/");

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Kategoriler = new SelectList(_context.Categories, "Id", "Name");
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            ViewBag.Kategoriler = new SelectList(_context.Categories, "Id", "Name");
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile? image, bool removeImg = false)
        {
            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                    {
                        category.Image = await FileHelper.FileLoaderAsync(image, "/Img/Categories/");
                    }
                    else if (removeImg)
                    {
                        category.Image = string.Empty;
                    }
                    else
                    {
                        // Görsel yüklenmedi, silme işaretlenmedi → mevcut görseli koru
                        _context.Entry(category).Property(x => x.Image).IsModified = false;
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Kategoriler = new SelectList(_context.Categories, "Id", "Name");
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            if (category.ParentId != null)
            {
                var parent = await _context.Categories.FindAsync(category.ParentId);
                ViewBag.ParentName = parent?.Name ?? "-";
            }
            else
            {
                ViewBag.ParentName = "-";
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                if (!string.IsNullOrEmpty(category.Image))
                {
                    FileHelper.FileRemover(category.Image, "/Img/Categories/");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
