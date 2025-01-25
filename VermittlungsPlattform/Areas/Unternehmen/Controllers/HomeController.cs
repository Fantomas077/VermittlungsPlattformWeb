using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Unternehmen.Controllers
{
    [Area("Unternehmen")]
    [Authorize(Roles = "unternehmen")]

    public class HomeController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;
        public HomeController( VermittlungsplattformDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var Company = _context.UnternehmenProfiles.OrderByDescending(x => x.Id).Take(8).ToList();
            ViewData["Company"] = Company;

            var Stelle = _context.PraktikumStelles.ToList();
            ViewData["Stelle"] = Stelle;
            var StelleBewerbung = _context.StelleBewerbungs.ToList();
            ViewData["StelleBewerbung"] = StelleBewerbung;


            var Student = _context.StudentProfiles.ToList();
            ViewData["Student"] = Student;
            var Userr = _context.Users.ToList();
            ViewData["Userr"] = Userr;
            return View();
        }
    }
}
