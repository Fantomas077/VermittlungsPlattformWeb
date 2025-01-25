using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
        [HttpGet]
        public IActionResult Apply(int Id)
        {
            PraktikumStelle? obj = _context.PraktikumStelles.FirstOrDefault(x => x.Id == Id);
            if (obj == null)
            {
                return NotFound();
            }
            var Company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = Company;
            ViewData["Stelle"] = _context.PraktikumStelles.Where(x => x.UnternehmenProfileId == Id).ToList();

            var Student = _context.StudentProfiles.ToList();
            ViewData["Student"] = Student;
            return View(obj);
        }
        [HttpPost]
        public IActionResult Apply(string anschreiben, int UnternehmenId, int StelleId)
        {
            // Vérifie si les champs requis sont présents
            if (!string.IsNullOrEmpty(anschreiben) && UnternehmenId != 0 && StelleId != 0)
            {
                // Récupérer les informations de l'utilisateur connecté via les claims
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdString == null)
                {
                    return Unauthorized();  // Gérer le cas où l'utilisateur n'est pas connecté
                }

                int userId = int.Parse(userIdString);  // Convertir l'ID utilisateur de string à int

                // Vérifier si l'utilisateur connecté correspond au profil étudiant
                var studentProfile = _context.StudentProfiles.FirstOrDefault(x => x.UserId == userId);
                if (studentProfile == null)
                {
                    return Unauthorized();  // Si l'utilisateur n'a pas de profil étudiant associé
                }

                // Créer une nouvelle instance de StelleBewerbung (Candidature)
                StelleBewerbung newBewerbung = new StelleBewerbung
                {
                    Anschreiben = anschreiben,
                    UnternhemenId = UnternehmenId,   // Correctement assigné
                    StelleId = StelleId,             // Correctement assigné
                    StudentProfilId = studentProfile.Id,  // Utilisation de l'ID du profil étudiant
                    ApplyDate = DateTime.Now,
                    Cv=studentProfile.Cvname,
                    Status = "Anstehend",            // Statut initial de la candidature
                    UserId = userId     
                    // ID de l'utilisateur connecté
                };

                // Ajoute la candidature dans la base de données
                _context.StelleBewerbungs.Add(newBewerbung);
                _context.SaveChanges();

                // Message de succès
                TempData["SuccessMessage"] = "Ihre Bewerbung wurde erfolgreich eingereicht.";
                return Redirect("/Home/");
            }

            // Si les champs sont manquants ou invalides
            TempData["ErrorMessage"] = "Bitte füllen Sie alle erforderlichen Felder aus.";
            return View(new StelleBewerbung());
        }


    }
}
