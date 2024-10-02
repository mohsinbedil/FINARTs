using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FineArt.Models.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore;

namespace FineArt.Controllers
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Manager + "," + SD.Role_Student + "," + SD.Role_Staff)]
    public class SubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _Webhost;

        public SubmissionsController(ApplicationDbContext context, IWebHostEnvironment whe)
        {
            _context = context;
            _Webhost = whe;
        }

        // GET: Submissions
        public async Task<IActionResult> Index()
        {
            return _context.Submissions != null ?
                        View(await _context.Submissions.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Submission'  is null.");
        }

        // GET: Submissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Submissions == null)
            {
                return NotFound();
            }

            var submission = await _context.Submissions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (submission == null)
            {
                return NotFound();
            }

            return View(submission);
        }

        public IActionResult Disqualified()
        {
            return View();
        } public IActionResult NotFound()
        {
            return View();
        }
        // GET: Submissions/Create
        public IActionResult Create()
        {
            var competitions = _context.Competitions.ToList();
            ViewBag.Competitions = competitions.Select(c => new SelectListItem { Value = c.Name, Text = c.Name }).ToList();

            return View();
        }

        // POST: Submissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576000)]
        [RequestSizeLimit(1048576000)]
        public IActionResult Create(Submission submit)
        {
            var competition = _context.Competitions.FirstOrDefault(c => c.Name == submit.Compet_Name);
            if (competition == null)
            {
                TempData["NotFound"] = "Event Not Found";
                return RedirectToAction("NotFound", "Submissions");
            }
            else
            {
                if (submit.SubmissionDate > competition.EndDate)
                {
                    return RedirectToAction("Disqualified"); // Submission date is beyond competition end date
                }

                if (ModelState.IsValid)
                {
                    
                    string filename = "";
                    if (submit.Design != null)
                    {
                        string uploadFolder = Path.Combine(_Webhost.WebRootPath, "Content/Images");
                        filename = Guid.NewGuid().ToString() + "  " + submit.Design.FileName;
                        string filePath = Path.Combine(uploadFolder, filename);
                        string extension = Path.GetExtension(submit.Design.FileName);

                        if (extension.ToLower() == ".jfif" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".webp" || extension.ToLower() == ".mp4")
                        {
                            submit.Design.CopyTo(new FileStream(filePath, FileMode.Create));

                            if (submit.Design.Length <= 1048576000)
                            {
                                Submission sub1 = new Submission
                                {
                                    Compet_Name = submit.Compet_Name,
                                    Stud_Name = submit.Stud_Name,
                                    Description = submit.Description,
                                    SubmissionDate = submit.SubmissionDate,
                                    Marks = submit.Marks,
                                    FeedBack = submit.FeedBack,
                                    DIMG = filename,
                                };

                                _context.Submissions.Add(sub1);
                                _context.SaveChanges();
                                TempData["success"] = "Record Inserted Successfully";
                                return RedirectToAction("Index", "Submissions");
                            }
                        }
                    }
                }
            }

            return View();
        }


        // GET: Submissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submissions.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }
            return View(submission);
        }

        // POST: Submissions/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, Submission Upsub)
        {
            if (ModelState.IsValid)
            {
                var sub = await _context.Submissions.FindAsync(id);
                string filename = "";
                if (Upsub.Design != null)
                {
                    string uploadfolder = Path.Combine(_Webhost.WebRootPath, "Content/Images");
                    filename = Guid.NewGuid().ToString() + "  " + Upsub.Design.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    string extension = Path.GetExtension(Upsub.Design.FileName);

                    if (extension.ToLower() == ".jfif" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".webp" || extension.ToLower() == ".mp4")
                    {
                        Upsub.Design.CopyTo(new FileStream(filepath, FileMode.Create));

                        if (Upsub.Design.Length <= 1048576000)
                        {

                            sub.Compet_Name = Upsub.Compet_Name;
                            sub.Stud_Name = Upsub.Stud_Name;
                            sub.Description = Upsub.Description;
                            sub.SubmissionDate = Upsub.SubmissionDate;
                            sub.Marks = Upsub.Marks;
                            sub.FeedBack = Upsub.FeedBack;
                            sub.DIMG = filename;

                            _context.Update(sub);
                            await _context.SaveChangesAsync();
                            TempData["success"] = "Record Inserted Successfully";
                            return RedirectToAction("Index", "Submissions");
                        }
                    }
                }
            }

            return View();

        }
        // GET: Submissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Submissions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Exhibitions'  is null.");
            }
            var SUB = await _context.Submissions.FindAsync(id);

            if (SUB != null)
            {
                if (!string.IsNullOrEmpty(SUB.DIMG))
                {
                    string imagePath = Path.Combine(_Webhost.WebRootPath, "Content/Images", SUB.DIMG);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Submissions.Remove(SUB);
                await _context.SaveChangesAsync();

                TempData["Deleted"] = "Record Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Product not found";
            return RedirectToAction(nameof(Index));
        }

        // POST: Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Submissions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Submission'  is null.");
            }
            var submission = await _context.Submissions.FindAsync(id);
            if (submission != null)
            {
                _context.Submissions.Remove(submission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubmissionExists(int id)
        {
            return (_context.Submissions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}


//public async Task<IActionResult> Edit(int id, [Bind("Id,Compet_Name,Stud_Name,Description,SubmissionDate,Marks,FeedBack,DIMG")] Submission submission)
//{
//    if (id != submission.Id)
//    {
//        return NotFound();
//    }

//    if (ModelState.IsValid)
//    {
//        try
//        {
//            _context.Update(submission);
//            await _context.SaveChangesAsync();
//        }
//        catch (DbUpdateConcurrencyException)
//        {
//            if (!SubmissionExists(submission.Id))
//            {
//                return NotFound();
//            }
//            else
//            {
//                throw;
//            }
//        }
//        return RedirectToAction(nameof(Index));
//    }
//    return View(submission);
//}
