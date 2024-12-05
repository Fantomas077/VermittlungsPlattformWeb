using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Controllers
{

    public class UnternehmenController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;
        public UnternehmenController( VermittlungsplattformDbContext context)
        {
            _context = context;
            
        }

        public IActionResult Index()
        {
            List<UnternehmenProfile> obj = _context.UnternehmenProfiles.OrderByDescending(x => x.Id).ToList();
            return View(obj);
        }
        public IActionResult Search(string Searchtext, string City)
        {
            // Commence par une requête de base incluant toutes les données
            var query = _context.PraktikumStelles.AsQueryable();
            var company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = company;

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
           
            // Renvoie la vue "Index" avec les résultats filtrés
            return View("Index", result);
        }
        public IActionResult CompanyDetails(int Id)
        {
            UnternehmenProfile? obj = _context.UnternehmenProfiles.FirstOrDefault(x => x.Id == Id);
            if (obj == null)
            {
                return NotFound();
            }
            ViewData["Stelle"] = _context.PraktikumStelles.Where(x => x.UnternehmenProfileId == Id).ToList();
            ViewData["gallery"] = _context.CompanyProfileGalleries.Where(x => x.CompanyProfileId == Id).ToList();
            return View(obj);
        }
    }
}
