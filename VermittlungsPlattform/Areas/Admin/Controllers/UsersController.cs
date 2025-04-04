﻿using System;
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
    public class UsersController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public UsersController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Vorname,Email,Password,IsAdmin,IsStudent,IsCompany,RegisterDate,RecoveryCode")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Vorname,Email,Password,IsAdmin,IsStudent,IsCompany,RegisterDate,RecoveryCode")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
               
                if (user.IsStudent)
                {
                    var studentProfile = await _context.StudentProfiles
                        .FirstOrDefaultAsync(sp => sp.UserId == user.Id); 
                    if (studentProfile != null)
                    {
                        _context.StudentProfiles.Remove(studentProfile);  
                    }

                    var applications = await _context.StelleBewerbungs
                        .Where(a => a.UserId == user.Id)
                        .ToListAsync();

                    foreach (var application in applications)
                    {
                        _context.StelleBewerbungs.Remove(application);  
                    }
                }

                if (user.IsCompany)
                {
                    var companyProfile = await _context.UnternehmenProfiles
                        .FirstOrDefaultAsync(c => c.UserId == user.Id);  
                    if (companyProfile != null)
                    {
                        _context.UnternehmenProfiles.Remove(companyProfile);  
                    }

                    var jobOffers = await _context.PraktikumStelles
                        .Where(j => j.UserId == user.Id)
                        .ToListAsync();

                    foreach (var jobOffer in jobOffers)
                    {
                        _context.PraktikumStelles.Remove(jobOffer);  
                    }
                }

                
                _context.Users.Remove(user);

              
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
