using Microsoft.AspNetCore.Mvc;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Controllers
{
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
           
            return View(obj);
        }
    }
}
