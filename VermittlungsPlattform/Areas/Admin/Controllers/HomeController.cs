using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="admin")]
    public class HomeController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;


        public HomeController( VermittlungsplattformDbContext context)
        {
            
            _context = context;
        }
        public IActionResult Index()
        {
            var Userr = _context.Users.ToList();
            ViewData["Userr"] = Userr;
            var Comment = _context.Comments.ToList();
            ViewData["Comment"] = Comment;
            var Stelle = _context.PraktikumStelles.ToList();
            ViewData["Stelle"] = Stelle;
            var Company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = Company;
            return View();
        }
    }
}
