using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FineArt.Models.Data;
using Microsoft.AspNetCore.Authorization;

namespace FineArt.Controllers
{
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Manager + "," + SD.Role_Student + "," + SD.Role_Staff)]
    public class CompetitionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public readonly IWebHostEnvironment _Webhost;



        public CompetitionsController(ApplicationDbContext context, IWebHostEnvironment whe)
        {
            _context = context;
            _Webhost = whe;
        }

        // GET: Competitions
        public async Task<IActionResult> Index()
        {
            return _context.Competitions != null ?
                        View(await _context.Competitions.ToListAsync()):
                        Problem("Entity set 'ApplicationDbContext.Competition'  is null.");
        }

        // GET: Competitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Competitions == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .FirstOrDefaultAsync(m => m.CId == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // GET: Competitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576000)]
        [RequestSizeLimit(1048576000)]
        public IActionResult Create(Competition compit)
        {
            if (ModelState.IsValid)
            {
                string filename = "";
                if (compit.Pic != null)
                {
                    string uploadfolder = Path.Combine(_Webhost.WebRootPath, "Content/Images");
                    filename = Guid.NewGuid().ToString() + "  " + compit.Pic.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    string extension = Path.GetExtension(compit.Pic.FileName);

                    if (extension.ToLower() == ".jfif" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".webp" || extension.ToLower() == ".mp4")
                    {
                        compit.Pic.CopyTo(new FileStream(filepath, FileMode.Create));

                        if (compit.Pic.Length <= 1048576000)
                        {
                            Competition newProduct = new Competition
                            {
                                Name = compit.Name,
                                StartDate = compit.StartDate,
                                EndDate = compit.EndDate,
                                Conditions = compit.Conditions,
                                AwardDetails = compit.AwardDetails,
                                IMG = filename,
                            };

                            _context.Competitions.Add(newProduct);
                            _context.SaveChanges();
                            TempData["success"] = "Record Inserted Successfully";
                            return RedirectToAction("Index", "Competitions");
                        }
                    }
                }
            }

            return View();
        }

        // GET: Competitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Competitions == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }
            return View(competition);
        }

        // POST: Competitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1048576000)]
        [RequestSizeLimit(1048576000)]
        public async Task<IActionResult> Edit(int id, Competition updatecompet)
        {

            if (ModelState.IsValid)
            {
                var data = await _context.Competitions.FindAsync(id);
                string filename = "";
                if (updatecompet.Pic != null)
                {
                    string uploadfolder = Path.Combine(_Webhost.WebRootPath, "Content/Images");
                    filename = Guid.NewGuid().ToString() + "  " + updatecompet.Pic.FileName;
                    string filepath = Path.Combine(uploadfolder, filename);
                    string extension = Path.GetExtension(updatecompet.Pic.FileName);

                    if (extension.ToLower() == ".jfif" || extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".webp" || extension.ToLower() == ".mp4")
                    {
                        updatecompet.Pic.CopyTo(new FileStream(filepath, FileMode.Create));

                        if (updatecompet.Pic.Length <= 1048576000)
                        {

                            data.Name = updatecompet.Name;
                            data.StartDate = updatecompet.StartDate;
                            data.EndDate = updatecompet.EndDate;
                            data.Conditions = updatecompet.Conditions;
                            data.AwardDetails = updatecompet.AwardDetails;
                            data.IMG = filename;

                            _context.Update(data);
                            await _context.SaveChangesAsync();
                            TempData["success"] = "Record Inserted Successfully";
                            return RedirectToAction("Index", "Competitions");
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

        private async Task<IActionResult> Details(int id)
        {
            var employee = await _context.Competitions.FindAsync(id);
            return View(employee);
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Competitions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Exhibitions'  is null.");
            }
            var COMP = await _context.Competitions.FindAsync(id);

            if (COMP != null)
            {
                if (!string.IsNullOrEmpty(COMP.IMG))
                {
                    string imagePath = Path.Combine(_Webhost.WebRootPath, "Content/Images", COMP.IMG);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Competitions.Remove(COMP);
                await _context.SaveChangesAsync();

                TempData["Deleted"] = "Record Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Product not found";
            return RedirectToAction(nameof(Index));
        }


        private bool CompetitionExists(int id)
                {
                    return (_context.Competitions?.Any(e => e.CId == id)).GetValueOrDefault();
                }
            }
        }
    

