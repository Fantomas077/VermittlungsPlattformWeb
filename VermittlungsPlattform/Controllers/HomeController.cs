using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VermittlungsPlattform.Models;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VermittlungsplattformDbContext _context;
       

        public HomeController(ILogger<HomeController> logger, VermittlungsplattformDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var Company = _context.UnternehmenProfiles.OrderByDescending(x => x.Id).Take(8).ToList();
            ViewData["Company"] = Company;

            var Stelle = _context.PraktikumStelles.ToList();
            ViewData["Stelle"] = Stelle;


            var Student = _context.StudentProfiles.ToList();
            ViewData["Student"] = Student;

            return View();
        }
     

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
