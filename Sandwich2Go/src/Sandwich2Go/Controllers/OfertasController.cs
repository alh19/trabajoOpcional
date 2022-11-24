﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.OfertaViewModels;
using Sandwich2Go.Models.SandwichViewModels;

namespace Sandwich2Go.Controllers
{
    [Authorize]
    [Authorize(Roles = "Gerente")]
    public class OfertasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfertasController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Gerente")]
        // GET: Ofertas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Oferta
                .Include(p => p.Gerente)
                .Where(p => p.Gerente.Email == User.Identity.Name);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "Gerente")]
        // GET: Ofertas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // GET: Ofertas/Create
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Create(SelectedSandwichesForOfferViewModel selectedSandwiches)
        {
            OfertaCreateViewModel oferta = new OfertaCreateViewModel();
            oferta.OfertaSandwiches = new List<OfertaSandwichViewModel>();

            if (selectedSandwiches.IdsToAdd == null)
            {
                ModelState.AddModelError("SandwichNoSelected", "Debes elegir al menos un sándwich para crear una oferta.");
            }
            else
            {
                IList<string> idsSandwiches = selectedSandwiches.IdsToAdd.ToList();
                oferta.OfertaSandwiches = await _context.Sandwich
                    .Include(s => s.IngredienteSandwich).ThenInclude(ins => ins.Ingrediente)
                    .Where(s => idsSandwiches.Contains(s.Id.ToString()))
                    .Select(s => new OfertaSandwichViewModel(s))
                    .ToListAsync();
            }

            Gerente gerente = _context.Users.OfType<Gerente>().FirstOrDefault<Gerente>(c => c.UserName.Equals(User.Identity.Name));

            oferta.Nombre = gerente.Nombre;
            oferta.Apellido = gerente.Apellido;
            oferta.Email = gerente.Email;

            return View(oferta);
        }

        // POST: Ofertas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Gerente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(OfertaCreateViewModel ofertaViewModel)
        {
            Sandwich sandwich;
            OfertaSandwich ofertaSandwich;
            Gerente gerente;
            Oferta oferta = new();
            oferta.Nombre = "";
            oferta.OfertaSandwich = new List<OfertaSandwich>();
            gerente = await _context.Users.OfType<Gerente>().FirstOrDefaultAsync<Gerente>(c => c.UserName.Equals(User.Identity.Name));

            if (ModelState.IsValid)
            {
                foreach (OfertaSandwichViewModel sandwichO in ofertaViewModel.OfertaSandwiches)
                {
                    sandwich = await _context.Sandwich.FirstOrDefaultAsync<Sandwich>(s => s.Id == sandwichO.SandwichID);
                }
            }

            return RedirectToAction("Details", new { id = oferta.Id });
        }
        [Authorize(Roles = "Gerente")]
        // GET: Ofertas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferta.FindAsync(id);
            if (oferta == null)
            {
                return NotFound();
            }
            return View(oferta);
        }
        [Authorize(Roles = "Gerente")]
        // POST: Ofertas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,FechaInicio,FechaFin,Descripcion")] Oferta oferta)
        {
            if (id != oferta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oferta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfertaExists(oferta.Id))
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
            return View(oferta);
        }
        [Authorize(Roles = "Gerente")]
        // GET: Ofertas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oferta = await _context.Oferta
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oferta == null)
            {
                return NotFound();
            }

            return View(oferta);
        }

        // POST: Ofertas/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Gerente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oferta = await _context.Oferta.FindAsync(id);
            _context.Oferta.Remove(oferta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfertaExists(int id)
        {
            return _context.Oferta.Any(e => e.Id == id);
        }
    }
}
