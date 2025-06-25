using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.Service.Abstract;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBaseService<Brand> _brandService;
        private readonly IBaseService<Category> _categoryService;


        public ProductController(IProductService productService, IBaseService<Brand> brandService, IBaseService<Category> categoryService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var productContext = _productService.GetAllWithBrandAndCategoryAsync();
            return View(await _productService.GetAllAsync());
        }

        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var products = await _productService.FindByIdAsync(id);
            if (products == null)
                return NotFound("Ürün Bulunamadı.");

            return Ok(products);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _productService.GetByIdWithBrandAndCategoryAsync(id.Value);
            if (product == null)
                return NotFound();

            return View(product);
        }

        public IActionResult Create()
        {
            var brands = _brandService.GetAll();
            var categories = _categoryService.GetAll();

            ViewBag.BrandId = new SelectList(brands, "Id", "Name");
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image is not null)
                    product.Image = await FileHelper.FileLoaderAsync(image, "/Img/Products/");

                await _productService.AddAsync(product);
                await _productService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(_productService.GetAll(), "Id", "Name");
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.FindByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_productService.GetAll(), "Id", "Name");
            return View(product);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? image, bool removeImg = false)
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

                    _productService.Update(product);
                    await _productService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(_productService.GetAll(), "Id", "Name");
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var product = await _productService.FindByIdAsync(id);
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    FileHelper.FileRemover(product.Image, "/Img/Products/");
                }
                _productService.Delete(product);
            }
            await _productService.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}