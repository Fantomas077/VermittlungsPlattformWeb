using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Mail;
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
          
            var query = _context.PraktikumStelles.AsQueryable();

           
            if (!string.IsNullOrEmpty(Searchtext))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.Title, "%" + Searchtext + "%") ||
                    EF.Functions.Like(x.Tags, "%" + Searchtext + "%") ||
                    EF.Functions.Like(x.Branche, "%" + Searchtext + "%"));
            }

          
            if (!string.IsNullOrEmpty(City))
            {
                query = query.Where(x => EF.Functions.Like(x.Location, "%" + City + "%"));
            }

            
            var result = query.OrderBy(x => x.Title).ToList();
            var company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = company;
           
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
            var stellebewerbung = _context.StelleBewerbungs.ToList();
            ViewData["stellebewerbung"] = stellebewerbung;
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
           
            if (!string.IsNullOrEmpty(anschreiben) && UnternehmenId != 0 && StelleId != 0)
            {
                
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdString == null)
                {
                    return Unauthorized();  
                }

                int userId = int.Parse(userIdString);  

                
                var studentProfile = _context.StudentProfiles.FirstOrDefault(x => x.UserId == userId);
                if (studentProfile == null)
                {
                    return Unauthorized();  
                }

              
                StelleBewerbung newBewerbung = new StelleBewerbung
                {
                    Anschreiben = anschreiben,
                    UnternhemenId = UnternehmenId,   
                    StelleId = StelleId,             
                    StudentProfilId = studentProfile.Id,  
                    ApplyDate = DateTime.Now,
                    Cv=studentProfile.Cvname,
                    Status = "Anstehend",            
                    UserId = userId     
                    
                };

                
                _context.StelleBewerbungs.Add(newBewerbung);
                _context.SaveChanges();
                
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null || string.IsNullOrEmpty(user.Email))
                {
                    return BadRequest("L'adresse e-mail de l'utilisateur n'a pas été trouvée.");
                }

                
                var stelle = _context.PraktikumStelles.FirstOrDefault(x => x.Id == StelleId);
                var unternehmen = _context.UnternehmenProfiles.FirstOrDefault(x => x.Id == UnternehmenId);

               

                try
                {
                    // Configure email
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("nheltonn@gmail.com");
                    mail.To.Add(user.Email);  // use email user
                    mail.Subject = "Bewerbung erfolgreich gesendet";
                    mail.Body = $"Hallo {user.Name},\n\nIhre Bewerbung für die Stelle '{stelle.Title}' bei {unternehmen.Name} wurde erfolgreich eingereicht.\n\nMit freundlichen Grüßen,\nDas Team";

                    // Configure serveur SMTP
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("nheltonn@gmail.com", "ugju meqj olao siko");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    //---------------------------------------------
                    var stelle1 = _context.PraktikumStelles.FirstOrDefault(x => x.Id == StelleId);
                    var unternehmen1 = _context.UnternehmenProfiles.FirstOrDefault(x => x.Id == UnternehmenId);
                    var xo = _context.Users.FirstOrDefault(x => x.Id == unternehmen1.UserId);
                    MailMessage unternehmenMail = new MailMessage();
                    unternehmenMail.From = new MailAddress("nheltonn@gmail.com");
                    unternehmenMail.To.Add(xo.Email);  
                    unternehmenMail.Subject = "Neue Bewerbung eingegangen";
                    unternehmenMail.Body = $"Hallo {unternehmen1.Name},\n\nSie haben eine neue Bewerbung für die Stelle '{stelle1.Title}' erhalten.\n\nMit freundlichen Grüßen,\nDas Team";

                    // send email
                    SmtpServer.Send(unternehmenMail);


                }
                catch (Exception ex)
                {
                   
                    TempData["ErrorMessage"] = $"Ein Fehler ist aufgetreten: {ex.Message}";
                }

                return Redirect("/Home/");
            }

            
            TempData["ErrorMessage"] = "Bitte füllen Sie alle erforderlichen Felder aus.";
            return View(new StelleBewerbung());
        }


    }
}
