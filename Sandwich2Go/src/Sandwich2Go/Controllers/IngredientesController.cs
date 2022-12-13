using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.IngredienteViewModels;
using Microsoft.AspNetCore.Authorization;



namespace Sandwich2Go.Controllers
{
    [Authorize]
    public class IngredientesController : Controller
    {

        private readonly ApplicationDbContext _context;

        public IngredientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Gerente")]
        [HttpGet]
        public async Task<IActionResult> SelectIngrProvForPurchase(string? ingredienteNombre, int? ingredienteStock, int IdProveedor)
        {
            SelectIngrProvForPurchaseViewModel selectIngredientes = new SelectIngrProvForPurchaseViewModel();
            selectIngredientes.IdProveedor = IdProveedor;

            selectIngredientes.Ingredientes = _context.Ingrediente
                .Include(s => s.IngrProv).ThenInclude(p => p.Proveedor)
                .Where(s => (s.IngrProv
                    .Where(p => p.Proveedor.Id == IdProveedor).Any()
                    || IdProveedor.Equals(null)))
            .Where(s => (s.Nombre.Contains(ingredienteNombre) || ingredienteNombre == null)
            && (s.Stock <= ingredienteStock || ingredienteStock.Equals(null)))
            .Select(m => new IngrProvForPurchaseViewModel()
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Stock = m.Stock
            }).ToList();



            return View(selectIngredientes);
        }

        [Authorize(Roles = "Gerente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> SelectIngrProvForPurchase(SelectedIngrProvForPurchaseViewModel selectedIngrediente)
        {
            if (selectedIngrediente.IdsToAdd != null)
            {
                return RedirectToAction("Create", "PedidoProvs", selectedIngrediente);
            }
            
            //a message error will be shown to the customer in case no movies are selected
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos un ingrediente");

            //the View SelectMoviesForPurchase will be shown again
            return await SelectIngrProvForPurchase(selectedIngrediente.ingredienteNombre, 
                selectedIngrediente.ingredienteStock,
                selectedIngrediente.IdProveedor);
        }

        [HttpGet]
        public async Task<IActionResult> SelectIngredientesForPurchase(string ingredienteAlergenoSelected,
                string ingredienteNombre)
        {
          
            SelectIngredientesForPurchaseViewModel selectIngredientes = new SelectIngredientesForPurchaseViewModel();
            selectIngredientes.Alergenos =new SelectList(_context.Alergeno.Select(g => g.Name).ToList());

            selectIngredientes.Ingredientes = _context.Ingrediente
          .Include(i => i.AlergSandws).ThenInclude(asa => asa.Alergeno)
          .Where(s => (s.AlergSandws.Where(isa => isa.Ingrediente.AlergSandws
                  .Where(als => als.Alergeno.Name.Equals(ingredienteAlergenoSelected))
              .Any())
          .Count() == 0 || ingredienteAlergenoSelected == null) && (s.Nombre.Contains(ingredienteNombre) || ingredienteNombre == null))
          .Select(m => new IngredienteForPurchaseViewModel()
          {
               Id = m.Id,
               Nombre = m.Nombre,
               PrecioUnitario = m.PrecioUnitario,
               Stock = m.Stock,
          });
           
            

            selectIngredientes.Ingredientes = selectIngredientes.Ingredientes.ToList();



            return View(selectIngredientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> SelectIngredientesForPurchase(SelectedIngredientesForPurchaseViewModel
        selectedIngredientes)
        {

            if (selectedIngredientes.IdsToAdd != null)
            {
                
                return RedirectToAction("CreateSandwichPersonalizado", "Pedidos", selectedIngredientes);
            }

            ModelState.AddModelError(string.Empty, "You must select at least one ingrediente");

            return await SelectIngredientesForPurchase(selectedIngredientes.ingredienteAlergenoSelected, selectedIngredientes.ingredienteNombre);
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
