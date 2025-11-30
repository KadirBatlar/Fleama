using Fleama.Service.Abstract;
using Fleama.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fleama.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _blogService.GetAllBlogPostsAsync();
            return View(posts);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _blogService.GetBlogPostByIdAsync(id.Value);
            if (post == null)
                return NotFound();

            // Check if current user can edit/delete
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var isAdmin = User.IsInRole("Admin");
                ViewBag.CanEdit = await _blogService.CanUserEditOrDeleteAsync(post.Id, userId, isAdmin);
            }
            else
            {
                ViewBag.CanEdit = false;
            }

            return View(post);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateDTO createDto, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View(createDto);
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            FileDto? fileDto = null;
            if (image != null && image.Length > 0)
            {
                fileDto = await Utils.FileMapper.ToFileDtoAsync(image);
            }

            var result = await _blogService.CreateBlogPostAsync(createDto, fileDto, userId, isAdmin);
            return RedirectToAction(nameof(Detail), new { id = result.Id });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _blogService.GetBlogPostByIdAsync(id.Value);
            if (post == null)
                return NotFound();

            // Check authorization
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            if (!await _blogService.CanUserEditOrDeleteAsync(post.Id, userId, isAdmin))
            {
                return Forbid();
            }

            var updateDto = new BlogUpdateDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ExistingImageUrl = post.ImageUrl
            };

            return View(updateDto);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogUpdateDTO updateDto, IFormFile? image)
        {
            if (id != updateDto.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                updateDto.ExistingImageUrl = updateDto.ExistingImageUrl;
                return View(updateDto);
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            FileDto? fileDto = null;
            if (image != null && image.Length > 0)
            {
                fileDto = await Utils.FileMapper.ToFileDtoAsync(image);
            }

            var result = await _blogService.UpdateBlogPostAsync(id, updateDto, fileDto, userId, isAdmin);
            if (result == null)
                return NotFound();

            return RedirectToAction(nameof(Detail), new { id = result.Id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var post = await _blogService.GetBlogPostByIdAsync(id.Value);
            if (post == null)
                return NotFound();

            // Check authorization
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            if (!await _blogService.CanUserEditOrDeleteAsync(post.Id, userId, isAdmin))
            {
                return Forbid();
            }

            return View(post);
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole("Admin");

            var success = await _blogService.DeleteBlogPostAsync(id, userId, isAdmin);
            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}

