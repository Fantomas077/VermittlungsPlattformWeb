using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            return View("Index", obj); 
        }

        public IActionResult Search(string Searchtext, string City)
        {
            // Commence par une requête de base incluant toutes les données
            var query = _context.PraktikumStelles.AsQueryable();

            // Applique le filtre sur le titre, les tags ou la branche si le Searchtext n'est pas vide
            if (!string.IsNullOrEmpty(Searchtext))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.Title, "%" + Searchtext + "%") ||
                    EF.Functions.Like(x.Tags, "%" + Searchtext + "%") ||
                    EF.Functions.Like(x.Branche, "%" + Searchtext + "%"));
            }

            // Applique le filtre sur la ville si la City n'est pas vide
            if (!string.IsNullOrEmpty(City))
            {
                query = query.Where(x => EF.Functions.Like(x.Location, "%" + City + "%"));
            }

            // Trie les résultats par titre
            var result = query.OrderBy(x => x.Title).ToList();
            var company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = company;
            // Renvoie la vue "Index" avec les résultats filtrés
            return View("Index", result);
        }

        public IActionResult StelleDetails(int Id)
        {
           PraktikumStelle? obj = _context.PraktikumStelles.FirstOrDefault(x => x.Id == Id);
            if (obj == null)
            {
                return NotFound();
            }
            var Company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = Company;
            ViewData["Stelle"] = _context.PraktikumStelles.Where(x => x.UnternehmenProfileId == Id).ToList();
            return View(obj);
        }

    }
}
