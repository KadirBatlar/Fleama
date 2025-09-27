using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class NewsController : Controller
    {
        /*private readonly IBaseService<News> _service;

        public NewsController(IBaseService<News> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var newsList = await _service.GetAllAsync();
            return View(newsList ?? new List<News>());
        }

        public async Task<IActionResult> GetAll()
        {
            var news = await _service.GetAllAsync();
            return Ok(news);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var news = await _service.FindByIdAsync(id);
            if (news == null)
                return NotFound();

            return Ok(news);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var news = await _service.GetAsync(x => x.Id == id);
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
                    //news.Image = await FileHelper.FileLoaderAsync(image, "/Img/News/");

                _service.Add(news);
                await _service.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(news);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var news = await _service.FindByIdAsync(id.Value);
            if (news == null)
                return NotFound();

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

                    _service.Update(news);
                    await _service.SaveChangesAsync();
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
                return NotFound();

            var news = await _service.GetAsync(x => x.Id == id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var news = await _service.FindByIdAsync(id);
            if (news != null)
            {
                if (!string.IsNullOrEmpty(news.Image))
                {
                    FileHelper.FileRemover(news.Image, "/Img/News/");
                }

                _service.Delete(news);
                await _service.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }*/
    }
}