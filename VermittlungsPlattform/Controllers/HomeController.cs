using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VermittlungsPlattform.Models;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Controllers
{
    [Authorize]

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
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else if (User.IsInRole("unternehmen"))
            {
                return RedirectToAction("Index", "Home", new { area = "Unternehmen" });
            }



            var Company = _context.UnternehmenProfiles.OrderByDescending(x => x.Id).Take(8).ToList();
            ViewData["Company"] = Company;

            var Stelle = _context.PraktikumStelles.ToList();
            ViewData["Stelle"] = Stelle;


            var Student = _context.StudentProfiles.ToList();
            ViewData["Student"] = Student;
            var Userr = _context.Users.ToList();
            ViewData["Userr"] = Userr;
            var stinteresse = _context.StudentenInteresses.ToList();
            ViewData["stinteresse"] = stinteresse;
            var companyteresse = _context.CompanyInteresses.ToList();
            ViewData["companyteresse"] = companyteresse;


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
