using Microsoft.AspNetCore.Mvc;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Unternehmen.Controllers
{
    [Area("Unternehmen")]
    public class StudentController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;
        public StudentController(VermittlungsplattformDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            List<StudentProfile> obj = _context.StudentProfiles.OrderByDescending(x => x.Id).ToList();
            
            return View(obj);
        }
        public IActionResult StudentDetails(int Id)
        {
            StudentProfile? obj = _context.StudentProfiles.FirstOrDefault(x => x.Id == Id);
            if (obj == null)
            {
                return NotFound();
            }
            ViewData["user"] = _context.Users.ToList();
            var Student = _context.StudentProfiles.ToList();
            ViewData["Student"] = Student;


            return View(obj);
        }
    }
}
