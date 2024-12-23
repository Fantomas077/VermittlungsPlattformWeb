using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize]
    public class StudentProfilesController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public StudentProfilesController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Student/StudentProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudentProfiles.Where(x => x.UserId == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))).ToListAsync());
        }

        // GET: Student/StudentProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentProfile = await _context.StudentProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentProfile == null)
            {
                return NotFound();
            }

            return View(studentProfile);
        }

        // GET: Student/StudentProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/StudentProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Apropos,Fachrichtung,Studiengang,Schwerpunkte,Skills,Location,Geschlecht,Abschluss,Cvname,Instagram,Github,Linkedin,Facebook,Twitter")] StudentProfile studentProfile, IFormFile pdfFile)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdString == null)
                {
                    return Unauthorized();  // Gérer le cas où l'utilisateur n'est pas connecté
                }

                int userId = int.Parse(userIdString);  // Convertir le string en int

                studentProfile.UserId = userId;  // Assigner l'ID utilisateur converti

                //--------saving main image----------
                if (pdfFile != null)
                {
                    studentProfile.Cvname = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(pdfFile.FileName);
                    string fn;
                    fn = Directory.GetCurrentDirectory();
                    string ImagePath = Path.Combine(fn + "\\wwwroot\\uploads\\" + studentProfile.Cvname);


                    using (var stream = new FileStream(ImagePath, FileMode.Create))
                    {
                        pdfFile.CopyTo(stream);
                    }
                }
                //-----------------------------------



                _context.Add(studentProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentProfile);
        }

        // GET: Student/StudentProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentProfile = await _context.StudentProfiles.FindAsync(id);
            if (studentProfile == null)
            {
                return NotFound();
            }
            return View(studentProfile);
        }

        // POST: Student/StudentProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Apropos,Fachrichtung,Studiengang,Schwerpunkte,Skills,Location,Geschlecht,Abschluss,Cvname,Instagram,Github,Linkedin,Facebook,Twitter")] StudentProfile studentProfile)
        {
            if (id != studentProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentProfileExists(studentProfile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentProfile);
        }

        // GET: Student/StudentProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentProfile = await _context.StudentProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentProfile == null)
            {
                return NotFound();
            }

            return View(studentProfile);
        }

        // POST: Student/StudentProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentProfile = await _context.StudentProfiles.FindAsync(id);
            if (studentProfile != null)
            {
                _context.StudentProfiles.Remove(studentProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentProfileExists(int id)
        {
            return _context.StudentProfiles.Any(e => e.Id == id);
        }
    }
}
