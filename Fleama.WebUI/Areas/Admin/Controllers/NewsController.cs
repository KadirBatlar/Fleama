using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Fleama.Service.Concrete;
using Fleama.Shared.Dtos;
using Fleama.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleama.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _newsService.GetAllNewsAsync());
        }

        public async Task<IActionResult> GetAll()
        {
            var news = await _newsService.GetAllNewsAsync();
            return Ok(news);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
                return NotFound();

            return Ok(news);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null)
                return NotFound();

            var news = await _newsService.GetNewsByIdAsync(id);
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
            if (!ModelState.IsValid)
            {
                return View();                
            }
            var fileDto = new FileDto();
            if (image != null)
            {
                fileDto = await FileMapper.ToFileDtoAsync(image!);
            }
            await _newsService.CreateNewsAsync(news, fileDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();

            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, News news, IFormFile? image, bool removeImg)
        {
            if (id != news.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var fileDto = new FileDto();
                if (image != null)
                {
                    fileDto = await FileMapper.ToFileDtoAsync(image!);
                }
                var result = await _newsService.EditNewsAsync(id, news, fileDto, removeImg);
                if (result == null)
                    return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return NotFound();

            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var success = await _newsService.DeleteNewsAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}