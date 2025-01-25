using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Student.Controllers
{

    [Area("Student")]
    [Authorize(Roles = "student")]
    public class MeineBewerbungController : Controller
    {

        private readonly VermittlungsplattformDbContext _context;

        public MeineBewerbungController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null)
            {
                return Unauthorized();  
            }

            int userId;
            if (!int.TryParse(userIdString, out userId))
            {
                return BadRequest("Invalid user ID format.");  
            }

            var Studentprofil = await _context.StudentProfiles.FirstOrDefaultAsync(x => x.UserId == userId);

            if (Studentprofil == null)
            {
                return NotFound("Company profile not found.");  
            }

           
            var bewerbungen = await _context.StelleBewerbungs
                .Where(x => x.StudentProfilId == Studentprofil.Id)
                .ToListAsync();
            var Userr = _context.Users.ToList();
            ViewData["Userr"] = Userr;
            var Stelle = _context.PraktikumStelles.ToList();
            ViewData["Stelle"] = Stelle;
            var Company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = Company;
            return View(bewerbungen);  
        }
    }
}
