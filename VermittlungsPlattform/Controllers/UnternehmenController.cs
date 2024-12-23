using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;
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
            var query = _context.UnternehmenProfiles.AsQueryable();

            // Applique le filtre sur le titre, les tags ou la branche si le Searchtext n'est pas vide
            if (!string.IsNullOrEmpty(Searchtext))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.Name, "%" + Searchtext + "%") ||
                    EF.Functions.Like(x.Branche, "%" + Searchtext + "%"));
                  
            }

            // Applique le filtre sur la ville si la City n'est pas vide
            if (!string.IsNullOrEmpty(City))
            {
                query = query.Where(x => EF.Functions.Like(x.Location, "%" + City + "%"));
            }

            // Trie les résultats par titre
            var result = query.OrderBy(x => x.Name).ToList();
            var company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = company;
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
            ViewData["gallery"] = _context.CompanyGalleries.Where(x => x.CompanyProfileId == Id).ToList();
            //-------------Data comment---- -
            ViewData["comment"] = _context.Comments.Where(x => x.UnternehmenId == Id).OrderByDescending(x => x.CreateDate).ToList();
            ViewData["user"] = _context.Users.ToList();

            return View(obj);
        }
        [HttpPost]
        public IActionResult SubmitComment(string comment, int UnternehmenId)
        {
            // Vérifie si les champs requis sont présents
            if (!string.IsNullOrEmpty(comment) && UnternehmenId != 0)
            {
                // Récupérer les informations de l'utilisateur connecté via les claims
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdString == null)
                {
                    return Unauthorized();  // Gérer le cas où l'utilisateur n'est pas connecté
                }
                int userId = int.Parse(userIdString);  // Convertir le string en int



                // Créer un nouveau commentaire
                Comment newComment = new Comment
                {
                    CommentText = comment,
                    UnternehmenId = UnternehmenId,
                    CreateDate = DateTime.Now,
                    UserId = userId // Associe le nom de l'utilisateur récupéré
                };

                // Ajoute le commentaire dans la base de données
                _context.Comments.Add(newComment);
                _context.SaveChanges();

                // Message de succès
                TempData["SuccessMessage"] = "Votre commentaire a été soumis avec succès.";
                return Redirect("/Unternehmen/CompanyDetails/" + UnternehmenId);


            }
            else
            {
                // Message d'erreur en cas d'information incomplète
                TempData["ErrorMessage"] = "Veuillez compléter toutes les informations.";
                return Redirect("/Unternehmen/CompanyDetails/" + UnternehmenId);
            }
        }

    }
}
