using Fleama.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fleama.WebUI.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IUserFavoriteService _favoriteService;

        public FavoritesController(IUserFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        /// <summary>
        /// Display user's favorite products
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return RedirectToAction("SignIn", "Account");
            }

            var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
            return View(favorites);
        }

        /// <summary>
        /// Toggle favorite status (add/remove) - AJAX endpoint
        /// </summary>
        [HttpPost]
        //[IgnoreAntiforgeryToken]
        public async Task<IActionResult> Toggle(int productId)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Json(new { success = false, message = "Giriş yapmalısınız" });
            }

            try
            {
                bool isFavorited = await _favoriteService.ToggleFavoriteAsync(userId, productId);
                return Json(new { success = true, isFavorited = isFavorited });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        /// <summary>
        /// Check if product is favorited - AJAX endpoint
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> IsFavorite(int productId)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Json(new { isFavorited = false });
            }

            bool isFavorited = await _favoriteService.IsFavoriteAsync(userId, productId);
            return Json(new { isFavorited = isFavorited });
        }
    }
}
