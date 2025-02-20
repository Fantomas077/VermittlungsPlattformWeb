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
    [Authorize(Roles = "unternehmen")]
    public class PraktikumStellesController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public PraktikumStellesController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Unternehmen/PraktikumStelles
        public async Task<IActionResult> Index()
        {
            return View(await _context.PraktikumStelles.Where(x=>x.UserId== Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))).ToListAsync());
        }

        // GET: Unternehmen/PraktikumStelles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var praktikumStelle = await _context.PraktikumStelles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (praktikumStelle == null)
            {
                return NotFound();
            }

            return View(praktikumStelle);
        }

        // GET: Unternehmen/PraktikumStelles/Create
        public IActionResult Create()
        {
            ViewBag.Interres = _context.Interesses.ToList();
            return View();
        }

        // POST: Unternehmen/PraktikumStelles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,UnternehmenProfileId,Title,Description,Location,Branche,Dauer,Skills,CreateDate,Tags,Arbeitsyp,Gehalt")] PraktikumStelle praktikumStelle, List<int> SelectedInterests)
        {
            if (ModelState.IsValid)
            {
                
                var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Par exemple, UserId de l'utilisateur connecté
                var unternehmenProfile = await _context.UnternehmenProfiles
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (unternehmenProfile == null)
                {
                    // Gérer le cas où l'utilisateur n'a pas de profil d'entreprise
                    return BadRequest("No company profile found for this user.");
                }


                // Assigner l'ID du profil de l'entreprise
                praktikumStelle.UnternehmenProfileId = unternehmenProfile.Id;
                praktikumStelle.UserId = userId;
                praktikumStelle.CreateDate=DateTime.Now;
                _context.Add(praktikumStelle);
                await _context.SaveChangesAsync();

                // Enregistrer les intérêts sélectionnés
                foreach (var interestId in SelectedInterests)
                {
                    var interesse = new CompanyInteresse
                    {
                        UnternehmenprofilId = praktikumStelle.Id, 
                        UnternehmenInteresse = _context.Interesses.FirstOrDefault(i => i.Id == interestId)?.Name // Le nom de l'intérêt
                    };
                    _context.CompanyInteresses.Add(interesse);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(praktikumStelle);
        }

        // GET: Unternehmen/PraktikumStelles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var praktikumStelle = await _context.PraktikumStelles.FindAsync(id);
            if (praktikumStelle == null)
            {
                return NotFound();
            }

            ViewBag.Interres = _context.Interesses.ToList();

            // Récupérer les intérêts déjà sélectionnés pour ce profil
            var selectedInterests = _context.CompanyInteresses
                                             .Where(si => si.UnternehmenprofilId == id)
                                             .Select(si => si.Id)
                                             .ToList();

            // Passer les intérêts sélectionnés à la vue
            ViewBag.SelectedInterests = selectedInterests;
            return View(praktikumStelle);
        }

        // POST: Unternehmen/PraktikumStelles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,UnternehmenProfileId,Title,Description,Location,Branche,Dauer,Skills,CreateDate,Tags,Arbeitsyp,Gehalt")] PraktikumStelle praktikumStelle, List<int> SelectedInterests)
        {
            if (id != praktikumStelle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                { // Récupérer l'ID de l'utilisateur connecté et attribuer à UnternehmenProfileId
                    var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Par exemple, UserId de l'utilisateur connecté
                    var unternehmenProfile = await _context.UnternehmenProfiles
                        .FirstOrDefaultAsync(p => p.UserId == userId);

                    if (unternehmenProfile == null)
                    {
                        // Gérer le cas où l'utilisateur n'a pas de profil d'entreprise
                        return BadRequest("No company profile found for this user.");
                    }
                    var currentInterests = _context.CompanyInteresses.Where(si => si.UnternehmenprofilId == unternehmenProfile.Id).ToList();
                    _context.CompanyInteresses.RemoveRange(currentInterests);

                    // Ajouter les nouveaux intérêts
                    if (SelectedInterests != null && SelectedInterests.Any())
                    {
                        foreach (var interestId in SelectedInterests)
                        {
                            var interest = await _context.Interesses.FindAsync(interestId);
                            if (interest != null)
                            {
                                var studentInterest = new CompanyInteresse
                                {
                                    UnternehmenInteresse =interest.Name,
                                     UnternehmenprofilId= praktikumStelle.Id // Utilisation de l'objet `interest` déjà récupéré
                                };
                                _context.CompanyInteresses.Add(studentInterest);
                            }
                        }
                    }

                    // Assigner l'ID du profil de l'entreprise
                    praktikumStelle.UnternehmenProfileId = unternehmenProfile.Id;
                    praktikumStelle.UserId = userId;
                    praktikumStelle.CreateDate = DateTime.Now;
                    _context.Update(praktikumStelle);
                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PraktikumStelleExists(praktikumStelle.Id))
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
            return View(praktikumStelle);
        }

        // GET: Unternehmen/PraktikumStelles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var praktikumStelle = await _context.PraktikumStelles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (praktikumStelle == null)
            {
                return NotFound();
            }

            return View(praktikumStelle);
        }

        // POST: Unternehmen/PraktikumStelles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var praktikumStelle = await _context.PraktikumStelles.FindAsync(id);
            if (praktikumStelle != null)
            {
                _context.PraktikumStelles.Remove(praktikumStelle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PraktikumStelleExists(int id)
        {
            return _context.PraktikumStelles.Any(e => e.Id == id);
        }
    }
}
