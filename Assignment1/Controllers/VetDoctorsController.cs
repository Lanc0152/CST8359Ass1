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
    public class VetDoctorsController : Controller
    {
        private readonly VetSystemDbContext _context;

        public VetDoctorsController(VetSystemDbContext context)
        {
            _context = context;
        }

        // GET: VetDoctors
        public async Task<IActionResult> Index()
        {
            return View(await _context.VetDoctors.ToListAsync());
        }

        // GET: VetDoctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vetDoctor = await _context.VetDoctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vetDoctor == null)
            {
                return NotFound();
            }

            return View(vetDoctor);
        }

        // GET: VetDoctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VetDoctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Specialty")] VetDoctor vetDoctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vetDoctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vetDoctor);
        }

        // GET: VetDoctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vetDoctor = await _context.VetDoctors.FindAsync(id);
            if (vetDoctor == null)
            {
                return NotFound();
            }
            return View(vetDoctor);
        }

        // POST: VetDoctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Specialty")] VetDoctor vetDoctor)
        {
            if (id != vetDoctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vetDoctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VetDoctorExists(vetDoctor.Id))
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
            return View(vetDoctor);
        }

        // GET: VetDoctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vetDoctor = await _context.VetDoctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vetDoctor == null)
            {
                return NotFound();
            }

            return View(vetDoctor);
        }

        // POST: VetDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vetDoctor = await _context.VetDoctors.FindAsync(id);
            if (vetDoctor != null)
            {
                _context.VetDoctors.Remove(vetDoctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VetDoctorExists(int id)
        {
            return _context.VetDoctors.Any(e => e.Id == id);
        }
    }
}
