﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.SandwichViewModels;

namespace Sandwich2Go.Controllers
{
    public class SandwichesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SandwichesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sandwiches
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sandwich.ToListAsync());
        }

        [HttpGet]
        public IActionResult SelectSandwichForPurchase(float sandwichPrecio, string alergenoSelected)
        {
            SelectSandwichesViewModel selectSandwiches = new SelectSandwichesViewModel();
            selectSandwiches.Alergenos = new SelectList(_context.Alergeno.Select(a => a.Name).ToList());

            selectSandwiches.Sandwiches = _context.Sandwich.
                Include(s => s.IngredienteSandwich)
                .ThenInclude(s => s.Ingrediente)
                .Where(s => s.Precio <= sandwichPrecio || sandwichPrecio==0);

            selectSandwiches.Sandwiches = selectSandwiches.Sandwiches.ToList();

            return View(selectSandwiches);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectSandwichForPurchase(SelectedSandwichesForPurchaseViewModel selectedSandwich)
        {
            if (selectedSandwich.IdsToAdd != null)
            {

                return RedirectToAction("Create", "Purchases", selectedSandwich);
            }
            //a message error will be shown to the customer in case no movies are selected
            ModelState.AddModelError(string.Empty, "You must select at least one movie");

            //the View SelectMoviesForPurchase will be shown again
            return SelectSandwichForPurchase(float.Parse(selectedSandwich.sandwichPrecio), selectedSandwich.sandwichAlergenoSelected);

        }

        // GET: Sandwiches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = await _context.Sandwich
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sandwich == null)
            {
                return NotFound();
            }

            return View(sandwich);
        }

        // GET: Sandwiches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sandwiches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SandwichName,Precio,Desc")] Sandwich sandwich)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sandwich);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sandwich);
        }

        // GET: Sandwiches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = await _context.Sandwich.FindAsync(id);
            if (sandwich == null)
            {
                return NotFound();
            }
            return View(sandwich);
        }

        // POST: Sandwiches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SandwichName,Precio,Desc")] Sandwich sandwich)
        {
            if (id != sandwich.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sandwich);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SandwichExists(sandwich.Id))
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
            return View(sandwich);
        }

        // GET: Sandwiches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sandwich = await _context.Sandwich
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sandwich == null)
            {
                return NotFound();
            }

            return View(sandwich);
        }

        // POST: Sandwiches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sandwich = await _context.Sandwich.FindAsync(id);
            _context.Sandwich.Remove(sandwich);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SandwichExists(int id)
        {
            return _context.Sandwich.Any(e => e.Id == id);
        }
    }
}
