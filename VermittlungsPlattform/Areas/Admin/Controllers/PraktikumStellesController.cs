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
    public class PraktikumStellesController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public PraktikumStellesController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Admin/PraktikumStelles
        public async Task<IActionResult> Index()
        {
            return View(await _context.PraktikumStelles.ToListAsync());
        }

        // GET: Admin/PraktikumStelles/Details/5
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

        // GET: Admin/PraktikumStelles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PraktikumStelles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,UnternehmenProfileId,Title,Description,Location,Branche,Dauer,Skills,CreateDate,Tags,Arbeitsyp,Gehalt")] PraktikumStelle praktikumStelle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(praktikumStelle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(praktikumStelle);
        }

        // GET: Admin/PraktikumStelles/Edit/5
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
            return View(praktikumStelle);
        }

        // POST: Admin/PraktikumStelles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,UnternehmenProfileId,Title,Description,Location,Branche,Dauer,Skills,CreateDate,Tags,Arbeitsyp,Gehalt")] PraktikumStelle praktikumStelle)
        {
            if (id != praktikumStelle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: Admin/PraktikumStelles/Delete/5
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

        // POST: Admin/PraktikumStelles/Delete/5
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
