using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.IngredienteViewModels;


namespace Sandwich2Go.Controllers
{
    public class IngredientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IngredientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SelectIngredientesForPurchase(string ingredienteAlergeno,
string ingredienteNombre)
        {
            SelectIngredientesViewModel selectIngredientes =
            new SelectIngredientesViewModel();

            selectIngredientes.Ingredientes = _context.Ingrediente.Where(a => a.Nombre.Contains(ingredienteNombre)).ToList();




            selectIngredientes.Alergenos = new SelectList(_context.Alergeno.Select(a => a.Name).ToList());
            
            List<int> idIng = _context.AlergSandws.Where(als => (_context.Alergeno
            .Where(al => al.Name == ingredienteAlergeno).Select(al => al.id).ToList()).Contains(als.AlergenoId)).Select(als => als.IngredienteId).ToList();
            selectIngredientes.Ingredientes = _context.Ingrediente
                .Where(s => !idIng.Contains(s.Id) || ingredienteAlergeno == null);

            return View(selectIngredientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectMoviesForPurchase(SelectIngredientesForPurchaseViewModel
        selectedIngredientes)
        {

            if (selectedIngredientes.IdsToAdd != null)
            {
                return RedirectToAction("Create", "Purchases", selectedIngredientes);
            }

            ModelState.AddModelError(string.Empty, "You must select at least one ingrediente");

            return SelectIngredientesForPurchase(selectedIngredientes.ingredienteNombre,
            selectedIngredientes.ingredienteAlergenoSelected);
        }

            // GET: Ingredientes
            public async Task<IActionResult> Index()
        {
            return View(await _context.Ingrediente.ToListAsync());
        }

        // GET: Ingredientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingrediente = await _context.Ingrediente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingrediente == null)
            {
                return NotFound();
            }

            return View(ingrediente);
        }

        // GET: Ingredientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ingredientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,PrecioUnitario,Stock")] Ingrediente ingrediente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingrediente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingrediente);
        }

        // GET: Ingredientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingrediente = await _context.Ingrediente.FindAsync(id);
            if (ingrediente == null)
            {
                return NotFound();
            }
            return View(ingrediente);
        }

        // POST: Ingredientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,PrecioUnitario,Stock")] Ingrediente ingrediente)
        {
            if (id != ingrediente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingrediente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredienteExists(ingrediente.Id))
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
            return View(ingrediente);
        }

        // GET: Ingredientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingrediente = await _context.Ingrediente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingrediente == null)
            {
                return NotFound();
            }

            return View(ingrediente);
        }

        // POST: Ingredientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingrediente = await _context.Ingrediente.FindAsync(id);
            _context.Ingrediente.Remove(ingrediente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredienteExists(int id)
        {
            return _context.Ingrediente.Any(e => e.Id == id);
        }
    }
}
