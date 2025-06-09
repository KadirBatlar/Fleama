using Fleama.Core.Entities;
using Fleama.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class AppUserController : Controller
    {

        private readonly DatabaseContext _context;

        public AppUserController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.AppUsers.ToListAsync());
        }
        
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Set<AppUser>().ToListAsync();
            return Ok(users);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Set<AppUser>().FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
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
                _context.AppUsers.Add(user);
                await _context.SaveChangesAsync();
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

            var user = await _context.AppUsers.FindAsync(id);
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
                    _context.Update(user);
                    await _context.SaveChangesAsync();
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

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user != null)
            {
                _context.AppUsers.Remove(user);
            }            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}