using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VermittlungsPlattform.Models.Db;

namespace VermittlungsPlattform.Areas.Unternehmen.Controllers
{
    [Area("Unternehmen")]
    public class CompanyProfilesController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public CompanyProfilesController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Unternehmen/CompanyProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompanyProfiles.ToListAsync());
        }

        // GET: Unternehmen/CompanyProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyProfile = await _context.CompanyProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyProfile == null)
            {
                return NotFound();
            }

            return View(companyProfile);
        }

        // GET: Unternehmen/CompanyProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Unternehmen/CompanyProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Branche,Description,Webseite,Imagename,Link")] CompanyProfile companyProfile, IFormFile? MainImage, IFormFile[]? GalleryImages)
        {
            if (ModelState.IsValid)
            {
                //--------saving main image----------
                if (MainImage != null)
                {
                    companyProfile.Imagename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(MainImage.FileName);
                    string fn;
                    fn = Directory.GetCurrentDirectory();
                    string ImagePath = Path.Combine(fn + "\\wwwroot\\images\\LogoCompany\\" + companyProfile.Imagename);


                    using (var stream = new FileStream(ImagePath, FileMode.Create))
                    {
                        MainImage.CopyTo(stream);
                    }
                }
                //-----------------------------------
                _context.Add(companyProfile);
                await _context.SaveChangesAsync();

                //============saving gallery images=====================
                if (GalleryImages != null)
                {
                    foreach (var item in GalleryImages)
                    {
                        var newgallery = new CompanyProfileGallery();
                        newgallery.CompanyProfileId = companyProfile.Id;
                        //------------------------
                        newgallery.ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(item.FileName);
                        string fn;
                        fn = Directory.GetCurrentDirectory();
                        string ImagePath = Path.Combine(fn + "\\wwwroot\\images\\LogoCompany\\" + newgallery.ImageName);

                        using (var stream = new FileStream(ImagePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }
                        //------------------------
                        _context.CompanyProfileGalleries.Add(newgallery);
                    }
                }
                await _context.SaveChangesAsync();
                //---------------------------------
                return RedirectToAction(nameof(Index));
            }
            return View(companyProfile);
        }

        // GET: Unternehmen/CompanyProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyProfile = await _context.CompanyProfiles.FindAsync(id);
            if (companyProfile == null)
            {
                return NotFound();
            }
            return View(companyProfile);
        }

        // POST: Unternehmen/CompanyProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Branche,Description,Webseite,Imagename,Link")] CompanyProfile companyProfile)
        {
            if (id != companyProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyProfileExists(companyProfile.Id))
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
            return View(companyProfile);
        }

        // GET: Unternehmen/CompanyProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyProfile = await _context.CompanyProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyProfile == null)
            {
                return NotFound();
            }

            return View(companyProfile);
        }

        // POST: Unternehmen/CompanyProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyProfile = await _context.CompanyProfiles.FindAsync(id);
            if (companyProfile != null)
            {
                _context.CompanyProfiles.Remove(companyProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyProfileExists(int id)
        {
            return _context.CompanyProfiles.Any(e => e.Id == id);
        }
    }
}
