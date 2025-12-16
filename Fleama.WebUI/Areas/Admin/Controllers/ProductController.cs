using Fleama.Core.Entities;
using Fleama.Core.Enums;
using Fleama.Service.Abstract;
using Fleama.Shared.Dtos;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var productContext = _productService.GetAllProductsAsync();
            return View(await productContext);
        }

        public async Task<IActionResult> Pending()
        {
            var pendingProducts = await _productService.GetPendingProductsAsync();
            return View(pendingProducts);
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

            var product = await _productService.GetProductByIdAsync(id.Value);
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
                var brands = _brandService.GetAll();
                var categories = _categoryService.GetAll();
                ViewBag.BrandId = new SelectList(brands, "Id", "Name");
                ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
                return View(product);
            }

            // Admin-created products are automatically approved
            product.ApproveStatus = ProductApproveStatus.Approved;
            product.IsActive = true;

            var fileDtos = new List<FileDto>();
            if (images != null)
            {
                foreach (var img in images.Where(x => x != null))
                    fileDtos.Add(await FileMapper.ToFileDtoAsync(img!));
            }

            await _productService.CreateProductAsync(product, fileDtos);

            TempData["Message"] = "Ürün başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _productService.UpdateProductStatusAsync(id, ProductApproveStatus.Approved);
            if (success)
            {
                TempData["Message"] = "Ürün onaylandı.";
            }
            else
            {
                TempData["Message"] = "Ürün bulunamadı.";
            }
            return RedirectToAction(nameof(Pending));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _productService.UpdateProductStatusAsync(id, ProductApproveStatus.Rejected);
            if (success)
            {
                TempData["Message"] = "Ürün reddedildi.";
            }
            else
            {
                TempData["Message"] = "Ürün bulunamadı.";
            }
            return RedirectToAction(nameof(Pending));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            
            var brands = _brandService.GetAll();
            var categories = _categoryService.GetAll();

            ViewBag.Brands = new SelectList(brands, "Id", "Name");            
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }


        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product, List<IFormFile?> images, List<int>? removeImgIds)
        {
            if (id != product.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_categoryService.GetAll(), "Id", "Name");
                ViewBag.Brands = new SelectList(_brandService.GetAll(), "Id", "Name");
                return View(product);
            }

            var fileDtos = new List<FileDto>();
            if (images != null)
            {
                foreach (var img in images.Where(x => x != null))
                    fileDtos.Add(await FileMapper.ToFileDtoAsync(img!));
            }

            // Preserve status if it was set, otherwise keep existing
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct != null)
            {
                product.ApproveStatus = existingProduct.ApproveStatus;
            }

            var result = await _productService.EditProductAsync(id, product, fileDtos, removeImgIds);
            if (result == null)
                return NotFound();

            TempData["Message"] = "Ürün başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}