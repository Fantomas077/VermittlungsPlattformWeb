using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VermittlungsPlattform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
