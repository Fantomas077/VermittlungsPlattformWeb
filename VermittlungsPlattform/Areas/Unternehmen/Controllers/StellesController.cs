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

namespace VermittlungsPlattform.Areas.Unternehmen.Controllers
{
    [Area("Unternehmen")]
    [Authorize]
    public class StellesController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public StellesController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Unternehmen/Stelles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stelles.ToListAsync());
        }

        // GET: Unternehmen/Stelles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stelle = await _context.Stelles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stelle == null)
            {
                return NotFound();
            }

            return View(stelle);
        }

        // GET: Unternehmen/Stelles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Unternehmen/Stelles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UnternehmenProfileId,Title,Description,Location,Branche,Duration,Skills,Tags,CreateDate,ArbeitsTyp,Gehalt")] Stelle stelle)
        {
            if (ModelState.IsValid)
            {
                // Récupérer l'ID de l'utilisateur connecté et attribuer à UnternehmenProfileId
                var userId =Convert.ToInt32( User.FindFirstValue(ClaimTypes.NameIdentifier)); // Par exemple, UserId de l'utilisateur connecté
                var unternehmenProfile = await _context.UnternehmenProfiles
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (unternehmenProfile == null)
                {
                    // Gérer le cas où l'utilisateur n'a pas de profil d'entreprise
                    return BadRequest("No company profile found for this user.");
                }

                // Assigner l'ID du profil de l'entreprise
                stelle.UnternehmenProfileId = unternehmenProfile.Id;

                // Assigner la date de création à la date et l'heure actuelles
                stelle.CreateDate = DateTime.Now;

                _context.Add(stelle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stelle);
        }

        // GET: Unternehmen/Stelles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stelle = await _context.Stelles.FindAsync(id);
            if (stelle == null)
            {
                return NotFound();
            }
            return View(stelle);
        }

        // POST: Unternehmen/Stelles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UnternehmenProfileId,Title,Description,Location,Branche,Duration,Skills,Tags,CreateDate,ArbeitsTyp,Gehalt")] Stelle stelle)
        {
            if (id != stelle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stelle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StelleExists(stelle.Id))
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
            return View(stelle);
        }

        // GET: Unternehmen/Stelles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stelle = await _context.Stelles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stelle == null)
            {
                return NotFound();
            }

            return View(stelle);
        }

        // POST: Unternehmen/Stelles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stelle = await _context.Stelles.FindAsync(id);
            if (stelle != null)
            {
                _context.Stelles.Remove(stelle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StelleExists(int id)
        {
            return _context.Stelles.Any(e => e.Id == id);
        }
    }
}
