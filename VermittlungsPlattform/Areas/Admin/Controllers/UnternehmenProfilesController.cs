using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UnternehmenProfilesController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public UnternehmenProfilesController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UnternehmenProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.UnternehmenProfiles.ToListAsync());
        }

        // GET: Admin/UnternehmenProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unternehmenProfile = await _context.UnternehmenProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unternehmenProfile == null)
            {
                return NotFound();
            }

            return View(unternehmenProfile);
        }

        // GET: Admin/UnternehmenProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/UnternehmenProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Name,Branche,Location,Description,Webseite,Link,ImageName")] UnternehmenProfile unternehmenProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unternehmenProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(unternehmenProfile);
        }

        // GET: Admin/UnternehmenProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unternehmenProfile = await _context.UnternehmenProfiles.FindAsync(id);
            if (unternehmenProfile == null)
            {
                return NotFound();
            }
            return View(unternehmenProfile);
        }

        // POST: Admin/UnternehmenProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,Branche,Location,Description,Webseite,Link,ImageName")] UnternehmenProfile unternehmenProfile)
        {
            if (id != unternehmenProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unternehmenProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnternehmenProfileExists(unternehmenProfile.Id))
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
            return View(unternehmenProfile);
        }

        // GET: Admin/UnternehmenProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unternehmenProfile = await _context.UnternehmenProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unternehmenProfile == null)
            {
                return NotFound();
            }

            return View(unternehmenProfile);
        }

        // POST: Admin/UnternehmenProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unternehmenProfile = await _context.UnternehmenProfiles.FindAsync(id);
            if (unternehmenProfile != null)
            {
                _context.UnternehmenProfiles.Remove(unternehmenProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnternehmenProfileExists(int id)
        {
            return _context.UnternehmenProfiles.Any(e => e.Id == id);
        }
    }
}
