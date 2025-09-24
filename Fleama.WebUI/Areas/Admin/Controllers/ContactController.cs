using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class ContactController : Controller
    {
        private readonly IBaseService<Contact> _service;

        public ContactController(IBaseService<Contact> service)
        {
            _service = service;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View(await _service.GetAllAsync());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _service.FindByIdAsync(id.Value);
            if (contact == null)
            {
                return NotFound();
            }
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
                    _service.Update(contact);
                    await _service.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(IndexAsync));
            }
            return View(contact);
        }
    }
}