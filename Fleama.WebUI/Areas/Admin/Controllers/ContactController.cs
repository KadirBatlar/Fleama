using Fleama.Core.Entities;
using Fleama.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class ContactController : Controller
    {
        private readonly DatabaseContext _context;

        public ContactController(DatabaseContext context)
        {
            _context = context;
        }

        // Admin panelinde iletişim formlarını listeleme
        public IActionResult Index()
        {
            var contacts = _context.Contacts
                                   .OrderByDescending(x => x.CreatedDate)
                                   .ToList();
            return View(contacts);
        }

        // Detay görüntüleme
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == id);
            if (contact == null)
                return NotFound();

            return View(contact);
        }

        // Yeni manuel kayıt oluşturma (Admin paneli için)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.UserName ??= "Admin"; // Admin panelinden ekleniyorsa
                contact.CreatedDate = DateTime.Now;
                contact.IsActive = true;
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(contact);
        }

        // Kullanıcı tarafı (frontend) üzerinden mesaj gönderme (örneğin: /Home/Contact)
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFromPublic(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.UserName = "Ziyaretçi";
                contact.CreatedDate = DateTime.Now;
                contact.IsActive = true;
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi.";
                return RedirectToAction("Contact", "Home");
            }

            return RedirectToAction("Contact", "Home");
        }

        // Düzenleme (gerekirse kullanılır)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();

            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Contact contact)
        {
            if (id != contact.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return View(contact);
        }

        // Silme ekranı
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
                return NotFound();

            return View(contact);
        }

        // Silme işlemi (onay sonrası)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
