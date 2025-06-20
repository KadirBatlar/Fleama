using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class AppUserController : Controller
    {

        private readonly IService<AppUser> _service;

        public AppUserController(IService<AppUser> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }
        
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _service.GetAsync(x => x.Id == id);
            if (user == null)
                return NotFound();

            return View(user);
        }
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUser user)
        {
            if(ModelState.IsValid)
            {
                _service.Add(user);
                await _service.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.FindByIdAsync(id.Value);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, AppUser user)
        {
            if (id != user.Id)
                return NotFound();

            if (ModelState.IsValid) 
            {
                try
                {
                    _service.Update(user);
                    await _service.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }        

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.GetAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var user = await _service.FindByIdAsync(id);
            if (user != null)
            {
                _service.Delete(user);
            }            
            await _service.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}