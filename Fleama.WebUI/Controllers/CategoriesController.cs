﻿using Fleama.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DatabaseContext _context;

        public CategoriesController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexAsync(int? id)
        {

            if (id == null)
                return NotFound();

            var category = await _context.Categories.Include(p => p.Products).FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }
    }
}