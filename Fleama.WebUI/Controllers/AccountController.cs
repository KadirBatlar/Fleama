using Fleama.Core.Entities;
using Fleama.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fleama.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;

        public AccountController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(AppUser appUser)
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AppUser appUser)
        {
            appUser.IsActive = true;
            appUser.IsAdmin = false;
            if(ModelState.IsValid)
            {
                await _context.AddAsync(appUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(appUser);
        }
    }
}