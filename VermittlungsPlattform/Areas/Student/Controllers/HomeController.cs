using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "student")]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            

            return View();
        }
    }
}
