using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Fleama.Service.Concrete;
using Fleama.Shared.Dtos;
using Fleama.WebUI.Models;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Controllers
{
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

        public async Task<IActionResult> Index(string search = "")
        {
            var productContext = _productService.GetAllAsync(p => p.IsActive && p.IsHome &&
                                     (p.Name.Contains(search) || p.Description.Contains(search)));

            return View(await productContext);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
                return NotFound();

            var model = new ProductDetailViewModel() { Product = product,
                RelatedProducts = _productService.GetQueryable().Where(p => p.IsActive && p.CategoryId == product.CategoryId && p.Id != product.Id)};
            return View(model);
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

            var fileDtos = new List<FileDto>();
            if (images != null)
            {
                foreach (var img in images.Where(x => x != null))
                    fileDtos.Add(await FileMapper.ToFileDtoAsync(img!));
            }

            await _productService.CreateProductAsync(product, fileDtos);

            return RedirectToAction(nameof(Index));
        }
    }
}