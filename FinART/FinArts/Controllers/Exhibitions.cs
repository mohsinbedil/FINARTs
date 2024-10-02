using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FineArt.Models.Data;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace FineArt.Controllers
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Manager + "," + SD.Role_Student + "," + SD.Role_Staff)]
    public class ExhibitionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public readonly IWebHostEnvironment _Webhost;

        public ExhibitionsController(ApplicationDbContext context, IWebHostEnvironment webhost)
        {
            _context = context;
            _Webhost = webhost;
        }

        // GET: Exhibitions
        public async Task<IActionResult> Index()
        {
            return _context.Exhibitions != null ?
                        View(await _context.Exhibitions.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Exhibitions'  is null.");
        }

        // GET: Exhibitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Exhibitions == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions
                .FirstOrDefaultAsync(m => m.Exid == id);
            if (exhibition == null)
            {
                return NotFound();
            }

            return View(exhibition);
        }

        // GET: Exhibitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exhibitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exhibition exhibition)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                if (exhibition.ExhibitionFile != null)
                {
                    string uploadfolder = Path.Combine(_Webhost.WebRootPath, "Content/Images");
                    filename = Guid.NewGuid().ToString() + "  " + exhibition.ExhibitionFile.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    string extension = Path.GetExtension(exhibition.ExhibitionFile.FileName);

                    if (extension.ToLower() == ".jfif" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".webp" || extension.ToLower() == ".mp4")
                    {
                        exhibition.ExhibitionFile.CopyTo(new FileStream(filepath, FileMode.Create));

                        if (exhibition.ExhibitionFile.Length <= 1048576000)
                        {
                            Exhibition EXPO = new Exhibition
                            {
                                Name = exhibition.Name,
                                StartDate = exhibition.StartDate,
                                EndDate = exhibition.EndDate,
                                ExhibitionIMG = filename,

                            };

                            _context.Exhibitions.Add(EXPO);
                            _context.SaveChanges();
                            TempData["success"] = "Record Inserted Successfully";
                            return RedirectToAction("Index", "Exhibitions");
                        }
                    }
                }
            }

            return View(exhibition);
        }

        // GET: Exhibitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Exhibitions == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition == null)
            {
                return NotFound();
            }
            return View(exhibition);
        }

        // POST: Exhibitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exhibition Updateexhibition)
        {

            if (ModelState.IsValid)
            {
                var data = await _context.Exhibitions.FindAsync(id);
                string filename = "";
                if (Updateexhibition.ExhibitionFile != null)
                {
                    string uploadfolder = Path.Combine(_Webhost.WebRootPath, "Content/Images");
                    filename = Guid.NewGuid().ToString() + "  " + Updateexhibition.ExhibitionFile.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    string extension = Path.GetExtension(Updateexhibition.ExhibitionFile.FileName);

                    if (extension.ToLower() == ".jfif" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".webp" || extension.ToLower() == ".mp4")
                    {
                        Updateexhibition.ExhibitionFile.CopyTo(new FileStream(filepath, FileMode.Create));

                        if (Updateexhibition.ExhibitionFile.Length <= 1048576000)
                        {

                            data.Name = Updateexhibition.Name;
                            data.StartDate = Updateexhibition.StartDate;
                            data.EndDate = Updateexhibition.EndDate;
                            data.ExhibitionIMG = filename;

                            _context.Update(data);
                            await _context.SaveChangesAsync();
                            TempData["success"] = "Record Inserted Successfully";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            TempData["error"] = "File Size is not Valid";

                        }
                    }
                    else
                    {
                        TempData["extension_error"] = "File Extension Not Valid";

                    }
                }

                return View();
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Exhibitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Exhibitions == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions
                .FirstOrDefaultAsync(m => m.Exid == id);
            if (exhibition == null)
            {
                return NotFound();
            }

            return View(exhibition);
        }

        // POST: Exhibitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Exhibitions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Exhibitions'  is null.");
            }
            var EXPO = await _context.Exhibitions.FindAsync(id);

            if (EXPO != null)
            {
                if (!string.IsNullOrEmpty(EXPO.ExhibitionIMG))
                {
                    string imagePath = Path.Combine(_Webhost.WebRootPath, "Content/Images", EXPO.ExhibitionIMG);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Exhibitions.Remove(EXPO);
                await _context.SaveChangesAsync();

                TempData["Deleted"] = "Record Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Product not found";
            return RedirectToAction(nameof(Index));
        }

        private bool ExhibitionExists(int id)
        {
            return (_context.Exhibitions?.Any(e => e.Exid == id)).GetValueOrDefault();
        }



        //ExhibitionPosting Actions
        [HttpGet]
        public IActionResult Addposting()
        {
            return View();
        }

        // POST: Exhibitions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Addposting(ExhibitionPosting exhibitionPosting)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                if (exhibitionPosting.EPFILE != null)
                {
                    string uploadfolder = Path.Combine(_Webhost.WebRootPath, "Content/Images");
                    filename = Guid.NewGuid().ToString() + "  " + exhibitionPosting.EPFILE.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    string extension = Path.GetExtension(exhibitionPosting.EPFILE.FileName);

                    if (extension.ToLower() == ".jfif" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".webp" || extension.ToLower() == ".mp4")
                    {
                        exhibitionPosting.EPFILE.CopyTo(new FileStream(filepath, FileMode.Create));

                        if (exhibitionPosting.EPFILE.Length <= 1048576000)
                        {
                            ExhibitionPosting EXPOPOSTING = new ExhibitionPosting
                            {
                                PaintingTitle = exhibitionPosting.PaintingTitle,
                                Price = exhibitionPosting.Price,
                                IsSold = exhibitionPosting.IsSold,
                                IsPaidToStudent = exhibitionPosting.IsPaidToStudent,
                                AmountPaid = exhibitionPosting.AmountPaid,
                                EPIMG = filename,

                            };

                            _context.ExhibitionPostings.Add(EXPOPOSTING);
                            _context.SaveChanges();
                            TempData["success"] = "Record Inserted Successfully";
                            return RedirectToAction(nameof(ViewPosting));
                        }
                    }
                }
            }


            return View(exhibitionPosting);
        }
       [HttpGet]
        public async Task<IActionResult> Editposting(int? id)
        {
            if (id == null || _context.ExhibitionPostings == null)
            {
                return NotFound();
            }

            var postings = await _context.ExhibitionPostings.FindAsync(id);
            if (postings == null)
            {
                return NotFound();
            }
            return View(postings);
        }

        // POST: Exhibitions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editposting(int id, ExhibitionPosting UpdatePosting)
        {
            if (ModelState.IsValid)
            {
                var EP = await _context.ExhibitionPostings.FindAsync(id);
                string filename = "";
                if (UpdatePosting.EPFILE != null)
                {
                    string uploadfolder = Path.Combine(_Webhost.WebRootPath, "Content/Images");
                    filename = Guid.NewGuid().ToString() + "  " + UpdatePosting.EPFILE.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    string extension = Path.GetExtension(UpdatePosting.EPFILE.FileName);

                    if (extension.ToLower() == ".jfif" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".webp" || extension.ToLower() == ".mp4")
                    {
                        UpdatePosting.EPFILE.CopyTo(new FileStream(filepath, FileMode.Create));

                        if (UpdatePosting.EPFILE.Length <= 1048576000)
                        {


                            EP.PaintingTitle = UpdatePosting.PaintingTitle;
                            EP.Price = UpdatePosting.Price;
                            EP.IsSold = UpdatePosting.IsSold;
                            EP.IsPaidToStudent = UpdatePosting.IsPaidToStudent;
                            EP.AmountPaid = UpdatePosting.AmountPaid;
                            EP.EPIMG = filename;



                            _context.ExhibitionPostings.Update(EP);
                            _context.SaveChanges();
                            TempData["success"] = "Record Inserted Successfully";
                            return RedirectToAction(nameof(ViewPosting));
                        }
                    }
                }
            }


            return View(UpdatePosting);
        }


        public async Task<IActionResult> DeletePosting(int? id)
        {
            if (id == null || _context.ExhibitionPostings == null)
            {
                return NotFound();
            }

            var exhibitionPosting = await _context.ExhibitionPostings
                .FirstOrDefaultAsync(m => m.ExPID == id);
            if (exhibitionPosting == null)
            {
                return NotFound();
            }

            return View(exhibitionPosting);
        }

        // POST: Exhibitions/Delete/5
        [HttpPost, ActionName("DeletePosting")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostingDeleteConfirmed(int id)
        {
            if (_context.Exhibitions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Exhibitions'  is null.");
            }
            var Post = await _context.ExhibitionPostings.FindAsync(id);

            if (Post != null)
            {
                if (!string.IsNullOrEmpty(Post.EPIMG))
                {
                    string imagePath = Path.Combine(_Webhost.WebRootPath, "Content/Images", Post.EPIMG);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.ExhibitionPostings.Remove(Post);
                await _context.SaveChangesAsync();

                TempData["Deleted"] = "Record Deleted Successfully";
                return RedirectToAction(nameof(ViewPosting));
            }

            TempData["Error"] = "Product not found";
            return RedirectToAction(nameof(ViewPosting));
        }
        [HttpGet]
        public async Task<IActionResult> ViewPosting()
        {
            {
                return _context.ExhibitionPostings != null ?
                            View(await _context.ExhibitionPostings.ToListAsync()) :
                            Problem("Entity set 'ApplicationDbContext.ExhibitionPOSTINGS'  is null.");
            }
        }
    }   
}
