using Fleama.Core.Entities;
using Fleama.Core.Enums;
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
            return View(await productContext);
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
        public async Task<IActionResult> Create(Product product, List<IFormFile?> images)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_productService.GetAll(), "Id", "Name");
                return View(product);
            }

            if (images != null && images.Any())
            {
                var filePaths = await FileHelper.FileLoaderMultipleAsync(images.Where(x => x != null)!, "/Img/Products/");

                product.Images = filePaths.Select(path => new Image
                {
                    Url = path,
                    ReferenceId = product.Id,   // ürünle ilişki kurulacak
                    ImageType = ImageType.Product // senin enum tipin
                }).ToList();
            }

            await _productService.AddAsync(product);
            await _productService.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, Product product, List<IFormFile?> images, bool removeImg = false)
        {
            if (id != product.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_productService.GetAll(), "Id", "Name");
                return View(product);
            }

            try
            {
                var existingProduct = await _productService.FindByIdAsync(id);
                if (existingProduct == null)
                    return NotFound();

                // Eğer removeImg seçilmişse eski görselleri sil
                if (removeImg && existingProduct.Images != null)
                {
                    foreach (var img in existingProduct.Images)
                    {
                        FileHelper.FileRemover(img.Url, "/Img/Products/");
                    }
                    existingProduct.Images.Clear();
                }

                // Yeni görseller yüklendiyse ekleme
                if (images != null && images.Any())
                {
                    var filePaths = await FileHelper.FileLoaderMultipleAsync(images.Where(x => x != null)!, "/Img/Products/");

                    var newImages = filePaths.Select(path => new Image
                    {
                        Url = path,
                        ReferenceId = product.Id,
                        ImageType = ImageType.Product
                    }).ToList();

                    // var olanlara ekleme
                    if (existingProduct.Images == null)
                        existingProduct.Images = newImages;
                    else
                        foreach (var img in newImages)
                            existingProduct.Images.Add(img);
                }

                existingProduct.Brand = product.Brand;
                existingProduct.BrandId = product.BrandId;
                existingProduct.OrderNo = product.OrderNo;
                existingProduct.ProductCode = product.ProductCode;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Category = product.Category;
                existingProduct.Description = product.Description;
                existingProduct.Name = product.Name;
                existingProduct.IsHome = product.IsHome;               

                _productService.Update(existingProduct);
                await _productService.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
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
                // Ürüne ait resimler varsa hepsini sil
                if (product.Images != null && product.Images.Any())
                {
                    foreach (var image in product.Images)
                    {
                        FileHelper.FileRemover(image.Url); // Url property’sini kullan
                    }
                }

                _productService.Delete(product);
                await _productService.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}