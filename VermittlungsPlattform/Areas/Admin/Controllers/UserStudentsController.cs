﻿using System;
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
    public class UserStudentsController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public UserStudentsController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UserStudents
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserStudents.ToListAsync());
        }

        // GET: Admin/UserStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userStudent = await _context.UserStudents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userStudent == null)
            {
                return NotFound();
            }

            return View(userStudent);
        }

        // GET: Admin/UserStudents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/UserStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Vorname,Email,Password,IsAdmin,RegisterDate,RecoveryCode")] UserStudent userStudent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userStudent);
        }

        // GET: Admin/UserStudents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userStudent = await _context.UserStudents.FindAsync(id);
            if (userStudent == null)
            {
                return NotFound();
            }
            return View(userStudent);
        }

        // POST: Admin/UserStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Vorname,Email,Password,IsAdmin,RegisterDate,RecoveryCode")] UserStudent userStudent)
        {
            if (id != userStudent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserStudentExists(userStudent.Id))
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
            return View(userStudent);
        }

        // GET: Admin/UserStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userStudent = await _context.UserStudents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userStudent == null)
            {
                return NotFound();
            }

            return View(userStudent);
        }

        // POST: Admin/UserStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userStudent = await _context.UserStudents.FindAsync(id);
            if (userStudent != null)
            {
                _context.UserStudents.Remove(userStudent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserStudentExists(int id)
        {
            return _context.UserStudents.Any(e => e.Id == id);
        }
    }
}
