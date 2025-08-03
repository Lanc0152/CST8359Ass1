using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Models;

namespace Assignment1.Controllers
{
    public class PetDetailsController : Controller
    {
        private readonly VetSystemDbContext _context;

        public PetDetailsController(VetSystemDbContext context)
        {
            _context = context;
        }

        // GET: PetDetails
        public async Task<IActionResult> Index()
        {
            var vetSystemDbContext = _context.PetDetails.Include(p => p.Pet);
            return View(await vetSystemDbContext.ToListAsync());
        }

        // GET: PetDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petDetails = await _context.PetDetails
                .Include(p => p.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (petDetails == null)
            {
                return NotFound();
            }

            return View(petDetails);
        }

        // GET: PetDetails/Create
        public IActionResult Create()
        {
            ViewData["PetId"] = _context.Pets
                .Select(p => new
                {
                    p.Id,
                    Display = p.Name + " (Microchip: " + p.MicrochipId + ")"
                })
                .ToList()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Display
                });

            return View();
        }


        // POST: PetDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VetNotes,PetId")] PetDetails petDetails, IFormFile noteFile)
        {
            // Step 1: If a file was uploaded, read it and populate VetNotes
            if (noteFile != null && noteFile.Length > 0)
            {
                using (var reader = new StreamReader(noteFile.OpenReadStream()))
                {
                    petDetails.VetNotes = await reader.ReadToEndAsync();
                }

                // Step 2: Remove model error if VetNotes is now filled
                if (!string.IsNullOrWhiteSpace(petDetails.VetNotes))
                {
                    ModelState.Remove(nameof(petDetails.VetNotes)); // "VetNotes" also works
                }
            }

            // Step 3: Validate + Save
            if (ModelState.IsValid)
            {
                _context.Add(petDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Rebuild dropdown
            ViewData["PetId"] = _context.Pets
                .Select(p => new
                {
                    p.Id,
                    Display = p.Name + " (Microchip: " + p.MicrochipId + ")"
                })
                .ToList()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Display,
                    Selected = (p.Id == petDetails.PetId)
                });

            return View(petDetails);
        }




        // GET: PetDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var petDetails = await _context.PetDetails
                    .Include(d => d.Pet)
                    .FirstOrDefaultAsync(d => d.Id == id);
            if (petDetails == null)
            {
                return NotFound();
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "MicrochipId", petDetails.PetId);
            return View(petDetails);
        }

        // POST: PetDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VetNotes,PetId")] PetDetails petDetails, IFormFile noteFile)
        {
            if (id != petDetails.Id)
            {
                return NotFound();
            }

            if (noteFile != null && noteFile.Length > 0)
            {
                using (var reader = new StreamReader(noteFile.OpenReadStream()))
                {
                    petDetails.VetNotes = await reader.ReadToEndAsync();
                }

                ModelState.Remove(nameof(petDetails.VetNotes));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PetDetails.Any(e => e.Id == petDetails.Id))
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

            ViewData["PetId"] = new SelectList(_context.Pets, "Id", "Name", petDetails.PetId);

            return View(petDetails);
        }


        // GET: PetDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petDetails = await _context.PetDetails
                .Include(p => p.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (petDetails == null)
            {
                return NotFound();
            }

            return View(petDetails);
        }

        // POST: PetDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var petDetails = await _context.PetDetails.FindAsync(id);
            if (petDetails != null)
            {
                _context.PetDetails.Remove(petDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetDetailsExists(int id)
        {
            return _context.PetDetails.Any(e => e.Id == id);
        }
    }
}
