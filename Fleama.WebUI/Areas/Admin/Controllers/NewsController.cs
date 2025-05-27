using Fleama.Core.Entities;
using Fleama.Data;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly DatabaseContext _context;

        public NewsController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.News.ToListAsync());
        }

        public async Task<IActionResult> GetAll()
        {
            var news = await _context.Set<News>().ToListAsync();
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var news = await _context.Set<News>().FindAsync(id);
            if (news == null)
                return NotFound();

            return Ok(news);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var news = await _context.News.FirstOrDefaultAsync(x => x.Id == id);
            if (news == null)
                return NotFound();

            return View(news);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(News news, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image is not null)
                    news.Image = await FileHelper.FileLoaderAsync(image, "/Img/News/");

                _context.News.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(news);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, News news, IFormFile? image, bool removeImg = false)
        {
            if (id != news.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (removeImg)
                        news.Image = string.Empty;

                    if (image is not null)
                        news.Image = await FileHelper.FileLoaderAsync(image, "/Img/News/");

                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FirstOrDefaultAsync(x => x.Id == id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                if (!string.IsNullOrEmpty(news.Image))
                {
                    FileHelper.FileRemover(news.Image, "/Img/News/");
                }

                _context.News.Remove(news);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}