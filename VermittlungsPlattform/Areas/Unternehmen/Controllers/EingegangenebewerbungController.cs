using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Unternehmen.Controllers
{
    [Area("Unternehmen")]
    [Authorize(Roles = "unternehmen")]
    public class EingegangenebewerbungController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public EingegangenebewerbungController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null)
            {
                return Unauthorized();  // Gérer le cas où l'utilisateur n'est pas connecté
            }

            int userId;
            if (!int.TryParse(userIdString, out userId))
            {
                return BadRequest("Invalid user ID format.");  // Gérer le cas où l'ID utilisateur n'est pas valide
            }

            // Trouver l'entreprise associée à l'utilisateur connecté
            var company = await _context.UnternehmenProfiles.FirstOrDefaultAsync(x => x.UserId == userId);

            if (company == null)
            {
                return NotFound("Company profile not found.");  // Gérer le cas où l'entreprise n'est pas trouvée
            }

            // Récupérer les candidatures reçues pour cette entreprise
            var bewerbungen = await _context.StelleBewerbungs
                .Where(x => x.UnternhemenId == company.Id)
                .ToListAsync();
            var Userr = _context.Users.ToList();
            ViewData["Userr"] = Userr;
            var Stelle = _context.PraktikumStelles.ToList();
            ViewData["Stelle"] = Stelle;
            var Company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = Company;
            return View(bewerbungen);  // Passer les candidatures à la vue
        }
    }
}
