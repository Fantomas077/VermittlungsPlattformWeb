using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
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
                return Unauthorized();  
            }

            int userId;
            if (!int.TryParse(userIdString, out userId))
            {
                return BadRequest("Invalid user ID format.");  
            }

            
            var company = await _context.UnternehmenProfiles.FirstOrDefaultAsync(x => x.UserId == userId);

            if (company == null)
            {
                return NotFound("Company profile not found.");  
            }

            
            var bewerbungen = await _context.StelleBewerbungs
                .Where(x => x.UnternhemenId == company.Id)
                .ToListAsync();
            var Userr = _context.Users.ToList();
            ViewData["Userr"] = Userr;
            var Stelle = _context.PraktikumStelles.ToList();
            ViewData["Stelle"] = Stelle;
            var Company = _context.UnternehmenProfiles.ToList();
            ViewData["Company"] = Company;
            return View(bewerbungen);  
        }
        public async Task<IActionResult> Zusagen(int id)
        {
            var bewerbung = await _context.StelleBewerbungs.FindAsync(id);
            if (bewerbung == null)
            {
                return NotFound();
            }

            bewerbung.Status = "Angenommen"; 
            _context.Update(bewerbung);
            await _context.SaveChangesAsync();

            var user = _context.StudentProfiles.FirstOrDefault(x => x.Id == bewerbung.StudentProfilId);
            var xo = _context.Users.FirstOrDefault(x => x.Id == user.UserId);
            if (xo == null || string.IsNullOrEmpty(xo.Email))
            {
                return BadRequest("Email adresse nicht gefunden.");
            }


           

            try
            {
                // Configure email
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("nheltonn@gmail.com");
                mail.To.Add(xo.Email);  // use email user
                mail.Subject = "Status Aktualisiert";
                mail.Body = $"Hallo {xo.Name},\n Die Status Ihrer bewerbung wurde Aktualisiert.\n\nMit freundlichen Grüßen,\nDas Team";

                // Configure serveur SMTP
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("nheltonn@gmail.com", "ugju meqj olao siko");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                


            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = $"Ein Fehler ist aufgetreten: {ex.Message}";
            }

            return RedirectToAction(nameof(Index)); 
        }

        public async Task<IActionResult> Absagen(int id)
        {
            var bewerbung = await _context.StelleBewerbungs.FindAsync(id);
            if (bewerbung == null)
            {
                return NotFound();
            }

            bewerbung.Status = "Abgelehnt"; 
            _context.Update(bewerbung);
            await _context.SaveChangesAsync();
            var user = _context.StudentProfiles.FirstOrDefault(x => x.Id == bewerbung.StudentProfilId);
            var xo = _context.Users.FirstOrDefault(x => x.Id == user.UserId);
            if (xo == null || string.IsNullOrEmpty(xo.Email))
            {
                return BadRequest("Email adresse nicht gefunden.");
            }




            try
            {
                // Configure email
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("nheltonn@gmail.com");
                mail.To.Add(xo.Email);  // use email user
                mail.Subject = "Status Aktualisiert";
                mail.Body = $"Hallo {xo.Name},\n Die Status Ihrer bewerbung wurde Aktualisiert.\n\nMit freundlichen Grüßen,\nDas Team";

                // Configure serveur SMTP
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("nheltonn@gmail.com", "ugju meqj olao siko");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);



            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = $"Ein Fehler ist aufgetreten: {ex.Message}";
            }


            return RedirectToAction(nameof(Index)); 
        }
    }
}
