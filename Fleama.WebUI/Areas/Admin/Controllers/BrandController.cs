using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class BrandController : Controller
    {
        private readonly IBaseService<Brand> _service;

        public BrandController(IBaseService<Brand> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        public async Task<IActionResult> GetAll()
        {
            var brands = await _service.GetAllAsync();
            return Ok(brands);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _service.FindByIdAsync(id);
            if (brand == null)
                return NotFound();

            return Ok(brand);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var brand = await _service.GetAsync(x => x.Id == id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _service.Add(brand);
                await _service.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _service.FindByIdAsync(id.Value);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Brand brand)
        {
            if (id != brand.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _service.Update(brand);
                    await _service.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _service.GetAsync(x => x.Id == id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var brand = await _service.FindByIdAsync(id);
            if (brand != null)
            {
                _service.Delete(brand);
            }
            await _service.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
    }
}