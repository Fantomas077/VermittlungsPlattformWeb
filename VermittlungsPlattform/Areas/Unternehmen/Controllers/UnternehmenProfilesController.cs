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
    [Authorize (Roles = "unternehmen")]
    public class UnternehmenProfilesController : Controller
    {
        private readonly VermittlungsplattformDbContext _context;

        public UnternehmenProfilesController(VermittlungsplattformDbContext context)
        {
            _context = context;
        }

        // GET: Unternehmen/UnternehmenProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.UnternehmenProfiles.Where(x => x.UserId == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))).ToListAsync());
        }

        public IActionResult DeleteGallery(int id)
        {
            var gallery = _context.CompanyProfileGalleries.FirstOrDefault(x => x.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }
            string d = Directory.GetCurrentDirectory();
            string fn = Path.Combine(d + "\\wwwroot\\images\\LogoCompany\\" + gallery.ImageName);
            

            if (System.IO.File.Exists(fn))
            {
                System.IO.File.Delete(fn);
            }
            _context.Remove(gallery);
            _context.SaveChanges();

            return Redirect("edit/" + gallery.CompanyProfileId);
        }

        // GET: Unternehmen/UnternehmenProfiles/Details/5
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
            //------------------
            ViewData["gallery"] = _context.CompanyProfileGalleries.Where(x => x.CompanyProfileId == unternehmenProfile.Id).ToList();
            //------------------
            return View(unternehmenProfile);
        }

        // GET: Unternehmen/UnternehmenProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Unternehmen/UnternehmenProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Name,Branche,Location,Description,Webseite,Link,ImageName")] UnternehmenProfile unternehmenProfile, IFormFile? MainImage, IFormFile[]? GalleryImages)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdString == null)
                {
                    return Unauthorized();  // Gérer le cas où l'utilisateur n'est pas connecté
                }

                int userId = int.Parse(userIdString);  // Convertir le string en int

                unternehmenProfile.UserId = userId;  // Assigner l'ID utilisateur converti

                //--------saving main image----------
                if (MainImage != null)
                {
                    unternehmenProfile.ImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(MainImage.FileName);
                    string fn;
                    fn = Directory.GetCurrentDirectory();
                    string ImagePath = Path.Combine(fn + "\\wwwroot\\images\\LogoCompany\\" + unternehmenProfile.ImageName);


                    using (var stream = new FileStream(ImagePath, FileMode.Create))
                    {
                        MainImage.CopyTo(stream);
                    }
                }
                //-----------------------------------



                _context.Add(unternehmenProfile);
                await _context.SaveChangesAsync();

                //============saving gallery images=====================
                if (GalleryImages != null)
                {
                    foreach (var item in GalleryImages)
                    {
                        var newgallery = new CompanyProfileGallery();
                        newgallery.CompanyProfileId = unternehmenProfile.Id;
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
            return View(unternehmenProfile);
        }

        // GET: Unternehmen/UnternehmenProfiles/Edit/5
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
            //------------------
            ViewData["gallery"] = _context.CompanyProfileGalleries.Where(x => x.CompanyProfileId == unternehmenProfile.Id).ToList();
            //------------------
            return View(unternehmenProfile);
        }

        // POST: Unternehmen/UnternehmenProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,Branche,Location,Description,Webseite,Link,ImageName")] UnternehmenProfile unternehmenProfile, IFormFile? MainImage, IFormFile[]? GalleryImages)
        {
            if (id != unternehmenProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (userIdString == null)
                    {
                        return Unauthorized();  // Gérer le cas où l'utilisateur n'est pas connecté
                    }

                    int userId = int.Parse(userIdString);  // Convertir le string en int

                    unternehmenProfile.UserId = userId;  // Assigner l'ID utilisateur converti
                    //***********save images*************
                    //========================================================
                    if (MainImage != null)
                    {
                        string d = Directory.GetCurrentDirectory();
                        string fn = Path.Combine(d + "\\wwwroot\\images\\LogoCompany\\" + unternehmenProfile.ImageName);
                        //------------------------------------------------
                        if (System.IO.File.Exists(fn))
                        {
                            System.IO.File.Delete(fn);
                        }
                        //------------------------------------------------
                        using (var stream = new FileStream(fn, FileMode.Create))
                        {
                            MainImage.CopyTo(stream);
                        }
                        //------------------------------------------------
                    }
                    //========================================================

                    //========================================================
                    if (GalleryImages != null)
                    {
                        foreach (var item in GalleryImages)
                        {

                            var imageName = Guid.NewGuid() + Path.GetExtension(item.FileName);
                            //------------------------------------------------
                            string d = Directory.GetCurrentDirectory();
                            string fn = Path.Combine(d + "\\wwwroot\\images\\LogoCompany\\" + imageName);
                            //------------------------------------------------
                            using (var stream = new FileStream(fn, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            //------------------------------------------------
                            var galleryItem = new CompanyProfileGallery();
                            galleryItem.ImageName = imageName;
                            galleryItem.CompanyProfileId = unternehmenProfile.Id;
                            //------------------------------------------------
                            _context.CompanyProfileGalleries.Add(galleryItem);
                        }
                    }
                    //========================================================
                    //***********************************
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

        // GET: Unternehmen/UnternehmenProfiles/Delete/5
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
            //------------------
            ViewData["gallery"] = _context.CompanyProfileGalleries.Where(x => x.CompanyProfileId == unternehmenProfile.Id).ToList();
            //------------------

            return View(unternehmenProfile);
        }

        // POST: Unternehmen/UnternehmenProfiles/Delete/5
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
