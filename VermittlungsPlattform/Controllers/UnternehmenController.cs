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
            var query = _context.UnternehmenProfiles.AsQueryable();

            if (!string.IsNullOrEmpty(Searchtext))
            {
                query = query.Where(x => EF.Functions.Like(x.Name, "%" + Searchtext + "%")
                                      || EF.Functions.Like(x.Branche, "%" + Searchtext + "%"));
            }

            if (!string.IsNullOrEmpty(City))
            {
                query = query.Where(x => EF.Functions.Like(x.Location, "%" + City + "%"));
            }

            var results = query.OrderBy(x => x.Name).ToList();


            return View("Index", results);
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
