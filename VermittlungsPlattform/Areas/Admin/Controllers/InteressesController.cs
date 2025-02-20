using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InteressesController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public InteressesController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Interesses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Interesses.ToListAsync());
        }

        // GET: Admin/Interesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interesse = await _context.Interesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (interesse == null)
            {
                return NotFound();
            }

            return View(interesse);
        }

        // GET: Admin/Interesses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Interesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Interesse interesse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(interesse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(interesse);
        }

        // GET: Admin/Interesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interesse = await _context.Interesses.FindAsync(id);
            if (interesse == null)
            {
                return NotFound();
            }
            return View(interesse);
        }

        // POST: Admin/Interesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Interesse interesse)
        {
            if (id != interesse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interesse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InteresseExists(interesse.Id))
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
            return View(interesse);
        }

        // GET: Admin/Interesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interesse = await _context.Interesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (interesse == null)
            {
                return NotFound();
            }

            return View(interesse);
        }

        // POST: Admin/Interesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interesse = await _context.Interesses.FindAsync(id);
            if (interesse != null)
            {
                _context.Interesses.Remove(interesse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InteresseExists(int id)
        {
            return _context.Interesses.Any(e => e.Id == id);
        }
    }
}
