using Fleama.Core.Entities;
using Fleama.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Fleama.WebUI.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly IBaseService<Category> _service;

        public Categories(IBaseService<Category> service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _service.GetAllAsync(c => c.IsActive && c.IsTopMenu));
        }
    }
}