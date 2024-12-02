using Microsoft.AspNetCore.Mvc;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Controllers
{
    public class StelleangeboteController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;
        public StelleangeboteController( VermittlungsplattformDbContext context)
        {
            _context = context;
            
        }
        public IActionResult Index()
        {
            List<PraktikumStelle>obj = _context.PraktikumStelles.OrderByDescending(x=>x.Id).ToList();

            var Company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = Company;
           

            return View(obj);
        }

        public IActionResult ByCompany(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id fehlt.");
            }

            List<PraktikumStelle> obj = _context.PraktikumStelles
                .Where(x => x.UnternehmenProfileId == id)
                .ToList();

            if (!obj.Any())
            {
                return NotFound("keine gefunden.");
            }

            var company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = company;

            return View("Index", obj); // On réutilise la même vue "Index" pour afficher les résultats filtrés
        }
    }
}
