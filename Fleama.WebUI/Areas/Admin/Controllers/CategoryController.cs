using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Fleama.Shared.Dtos;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            return View(await _categoryService.GetAllCategoriesAsync());
        }

        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
                return NotFound();

            // Show parent category name
            if (category.ParentId != null)
            {
                var parent = await _categoryService.GetCategoryByIdAsync(category.ParentId.Value);
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
            if (!ModelState.IsValid)
            {
                var categories = _categoryService.GetAll();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(category);
            }

            var fileDto = new FileDto();
            if (image != null)
            {
                fileDto = await FileMapper.ToFileDtoAsync(image!);
            }

            await _categoryService.CreateCategoryAsync(category, fileDto);

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryService.GetCategoryByIdAsync(id.Value);
            if (category == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "Id", "Name");
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile? image, bool removeImg)
        {
            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var fileDto = new FileDto();
                if (image != null)
                {
                    fileDto = await FileMapper.ToFileDtoAsync(image!);
                }
                var result = await _categoryService.EditCategoryAsync(id, category, fileDto, removeImg);
                if (result == null)
                    return NotFound();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "Id", "Name");            
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryService.GetCategoryByIdAsync(id);
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
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success) 
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

    }
}