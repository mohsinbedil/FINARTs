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
    [Authorize]
    public class AwardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AwardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Awards
        public async Task<IActionResult> Index()
        {
              return _context.Awards != null ? 
                          View(await _context.Awards.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Awards'  is null.");
        }

        // GET: Awards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Awards == null)
            {
                return NotFound();
            }

            var award = await _context.Awards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // GET: Awards/Create
        public IActionResult Create()
        {
            var competitions = _context.Competitions.ToList();
            ViewBag.Competitions = competitions.Select(c => new SelectListItem { Value = c.Name, Text = c.Name }).ToList();

            return View();
        }

        // POST: Awards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Stud_Name,Competition_Name,AwardDetails")] Award award)
        {
            if (ModelState.IsValid)
            {
                var competitionId = _context.Competitions
                                    .FirstOrDefault(c => c.Name == award.AwardDetails)?.CId;

                _context.Add(award);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If model state is not valid, re-render the view with the provided model
            var competitions = _context.Competitions.ToList();
            ViewBag.Competitions = competitions.Select(c => new SelectListItem { Value = c.Name, Text = c.Name }).ToList();
            return View(award);
        }
        // GET: Awards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Awards == null)
            {
                return NotFound();
            }
            var competitions = _context.Competitions.ToList();
            ViewBag.Competitions = competitions.Select(c => new SelectListItem { Value = c.Name, Text = c.Name }).ToList();

            var award = await _context.Awards.FindAsync(id);
            if (award == null)
            {
                return NotFound();
            }
            return View(award);
        }

        // POST: Awards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Stud_Name,Competition_Name,AwardDetails")] Award award)
        {
            if (id != award.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(award);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwardExists(award.Id))
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
            return View(award);
        }

        // GET: Awards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Awards == null)
            {
                return NotFound();
            }

            var award = await _context.Awards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // POST: Awards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Awards == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Awards'  is null.");
            }
            var award = await _context.Awards.FindAsync(id);
            if (award != null)
            {
                _context.Awards.Remove(award);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AwardExists(int id)
        {
          return (_context.Awards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
