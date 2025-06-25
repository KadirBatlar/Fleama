using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Fleama.WebUI.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace Fleama.WebUI.Controllers
{
    public class FavoritesContoller : Controller
    {
        private readonly IBaseService<Product> _service;

        public FavoritesContoller(IBaseService<Product> service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var favorites = GetFavorites();
            return View(favorites );
        }

        public List<Product> GetFavorites()
        {
            return HttpContext.Session.GetJson<List<Product>>("Favorites") ?? [];
        }

        public IActionResult Add(int productId)
        {
            var favorites = GetFavorites();
            var product = _service.FindById(productId);
            if (product != null && !favorites.Any(p => p.Id == productId)) 
            { 
                favorites.Add(product);
                HttpContext.Session.SetJson("Favorites", favorites);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            var favorites = GetFavorites();
            var product = _service.FindById(productId);
            if (product != null && favorites.Any(p => p.Id == productId)) 
            { 
                favorites.RemoveAll(p => p.Id == productId);
                HttpContext.Session.SetJson("Favorites", favorites);
            }
            return RedirectToAction("Index");
        }
    }
}