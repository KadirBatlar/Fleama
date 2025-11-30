using Fleama.Core.Entities;
using Fleama.Core.Enums;
using Fleama.Service.Abstract;
using Fleama.Service.Concrete;
using Fleama.Shared.Dtos;
using Fleama.WebUI.Models;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Fleama.WebUI.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBaseService<Brand> _brandService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, IBaseService<Brand> brandService, ICategoryService categoryService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string search = "")
        {
            // Show only approved and active products to all users
            var productContext = _productService.GetAllAsync(p => p.IsActive && 
                                     p.Status == ProductStatus.Approved &&
                                     (string.IsNullOrEmpty(search) || 
                                      p.Name.Contains(search) || 
                                      (p.Description != null && p.Description.Contains(search))));

            return View(await productContext);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
                return NotFound();

            var relatedProducts = _productService.GetQueryable()
                .Where(p => p.IsActive && 
                            p.Status == ProductStatus.Approved && 
                            p.Id != product.Id);
            
            // If product has a category, filter by it
            if (product.CategoryId.HasValue)
            {
                relatedProducts = relatedProducts.Where(p => p.CategoryId == product.CategoryId);
            }
            
            var model = new ProductDetailViewModel() 
            { 
                Product = product,
                RelatedProducts = relatedProducts
            };
            return View(model);
        }

        public IActionResult Create()
        {
            // Only allow normal users (not admins) to use this controller
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Create", "Product", new { area = "Admin" });
            }

            // User form doesn't need category/brand dropdowns
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, List<IFormFile?> images)
        {
            // Only allow normal users (not admins) to use this controller
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Create", "Product", new { area = "Admin" });
            }

            // Remove validation errors for CategoryId and BrandId since users don't provide them
            ModelState.Remove(nameof(Product.CategoryId));
            ModelState.Remove(nameof(Product.BrandId));

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            // Get current user ID
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                product.UserId = userId;
            }

            // Set status to Pending for user-submitted products
            product.Status = ProductStatus.Pending;
            product.IsActive = true;
            // Users cannot set these fields - they will be set by admin during approval
            product.IsHome = false;
            product.OrderNo = 0;
            product.CategoryId = null; // Admin will set this during approval
            product.BrandId = null; // Admin will set this during approval

            var fileDtos = new List<FileDto>();
            if (images != null)
            {
                foreach (var img in images.Where(x => x != null))
                    fileDtos.Add(await FileMapper.ToFileDtoAsync(img!));
            }

            await _productService.CreateProductAsync(product, fileDtos);

            TempData["Message"] = "Ürününüz başarıyla eklendi. Admin onayından sonra yayınlanacaktır.";
            return RedirectToAction(nameof(MyProducts));
        }

        public async Task<IActionResult> MyProducts()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return RedirectToAction(nameof(Index));
            }

            var myProducts = await _productService.GetProductsByUserIdAsync(userId);
            return View(myProducts);
        }
    }
}