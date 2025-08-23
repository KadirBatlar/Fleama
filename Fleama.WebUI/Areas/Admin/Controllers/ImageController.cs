using Fleama.Core.Entities;
using Fleama.Core.Enums;
using Fleama.Data;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class ImageController : Controller
    {
        private readonly DatabaseContext _context;

        public ImageController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Image
        public async Task<IActionResult> Index()
        {
            var images = await _context.Images.ToListAsync();
            return View(images);
        }

        // GET: Admin/Image/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var image = await _context.Images.FirstOrDefaultAsync(m => m.Id == id);
            if (image == null) return NotFound();

            return View(image);
        }

        // GET: Admin/Image/Create
        public IActionResult Create()
        {
            ViewData["ImageTypes"] = Enum.GetValues(typeof(ImageType));
            return View();
        }

        // POST: Admin/Image/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, ImageType imageType, int referenceId)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen bir dosya seçin.");
                return View();
            }

            var savedPath = await FileHelper.FileLoaderAsync(file, "Uploads");
            if (savedPath == null)
            {
                ModelState.AddModelError("", "Geçersiz dosya.");
                return View();
            }

            var image = new Image
            {
                Url = savedPath,
                ImageType = imageType,
                ReferenceId = referenceId
            };

            _context.Add(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Image/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            ViewData["ImageTypes"] = Enum.GetValues(typeof(ImageType));
            return View(image);
        }

        // POST: Admin/Image/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile? file, Image updatedImage)
        {
            if (id != updatedImage.Id) return NotFound();

            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Yeni dosya yüklenirse eskisini sil
                    if (file != null && file.Length > 0)
                    {
                        FileHelper.FileRemover(image.Url);
                        var savedPath = await FileHelper.FileLoaderAsync(file, "Uploads");
                        if (savedPath != null)
                            image.Url = savedPath;
                    }

                    image.ImageType = updatedImage.ImageType;
                    image.ReferenceId = updatedImage.ReferenceId;

                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Images.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(updatedImage);
        }

        // GET: Admin/Image/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var image = await _context.Images.FirstOrDefaultAsync(m => m.Id == id);
            if (image == null) return NotFound();

            return View(image);
        }

        // POST: Admin/Image/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image != null)
            {
                FileHelper.FileRemover(image.Url);
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}