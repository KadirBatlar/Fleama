using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.Service.Abstract;
using Fleama.Service.Concrete;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class CategoryController : Controller
    {        
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAllAsync());
        }

        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.FindByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryService.GetAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            // Show parent category name
            if (category.ParentId != null)
            {
                var parent = await _categoryService.FindByIdAsync(category.ParentId.Value);
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
            var categories = _categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                    category.Image = await FileHelper.FileLoaderAsync(image, "/Img/Categories/");

                await _categoryService.AddAsync(category);
                await _categoryService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var categories = _categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryService.FindByIdAsync(id.Value);
            if (category == null)
                return NotFound();

            var categories = _categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
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
                        _categoryService.UpdateCategoryWithoutModifyingImage(category);
                    }

                    _categoryService.Update(category);
                    await _categoryService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            var categories = _categoryService.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");            
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryService.GetAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            if (category.ParentId != null)
            {
                var parent = await _categoryService.FindByIdAsync(category.ParentId.Value);
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
            var category = await _categoryService.FindByIdAsync(id);
            if (category != null)
            {
                if (!string.IsNullOrEmpty(category.Image))
                {
                    FileHelper.FileRemover(category.Image, "/Img/Categories/");
                }

                _categoryService.Delete(category);
                await _categoryService.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}