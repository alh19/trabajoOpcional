using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.SandwichViewModels;

namespace Sandwich2Go.Controllers
{
    [Authorize]
    public class SandwichesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SandwichesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // GET: Sandwiches
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sandwich.ToListAsync());
        }

        [Authorize(Roles = "Cliente")]
        [HttpGet]
        public IActionResult SelectSandwichForPurchase(double sandwichPrecio, string sandwichAlergenoSelected)
        {
            SelectSandwichesViewModel selectSandwiches = new SelectSandwichesViewModel();
            selectSandwiches.Alergenos = new SelectList(_context.Alergeno.Select(a => a.Name).ToList());
            selectSandwiches.sandwichAlergenoSelected = sandwichAlergenoSelected;
            selectSandwiches.sandwichPrecio = sandwichPrecio;
            selectSandwiches.Sandwiches =  _context.Sandwich
                .Include(s => s.OfertaSandwich).ThenInclude(os => os.Oferta)
                .Include(s => s.IngredienteSandwich).ThenInclude(isa => isa.Ingrediente).ThenInclude(i => i.AlergSandws).ThenInclude(asa => asa.Alergeno)
                .Where(s => (s.IngredienteSandwich
                    .Where(isa => isa.Ingrediente.AlergSandws
                        .Where(als => als.Alergeno.Name.Equals(sandwichAlergenoSelected))
                    .Any())
                .Count() == 0 || sandwichAlergenoSelected == null) && //No muestro los sándwiches con el alérgeno indicado. Si no se indica no se aplica filtro.
                (s.Precio <= sandwichPrecio || sandwichPrecio == 0) &&//No muestro los sándwiches con precio superior al introducido. Si no se introduce precio o es 0 no se aplica filtro.
                (s.IngredienteSandwich.Where(isa => isa.Cantidad > (isa.Ingrediente.Stock))).Count()==0)//Tampoco los sándwiches que tengan ingredientes que necesiten
                .OrderBy(s=> s.SandwichName)                                                           //más cantidad que stock disponible. También incluye a ingredientes.stock = 0 por naturaleza. 
                .Where(s => !(s is SandwCreado)) //Para no mostrar los sándwiches personalizados en esta pantalla
                .Select(s=>new SandwichForPurchaseViewModel(s)).ToList();

            return View(selectSandwiches);
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectSandwichForPurchase(SelectedSandwichesForPurchaseViewModel selectedSandwich)
        {
            if (selectedSandwich.IdsToAdd != null)
            {
                return RedirectToAction("Create", "Pedidos", selectedSandwich);
            }
            //a message error will be shown to the customer in case no movies are selected
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos un Sándwich");

            //the View SelectMoviesForPurchase will be shown again
            return SelectSandwichForPurchase(double.Parse(selectedSandwich.sandwichPrecio), selectedSandwich.sandwichAlergenoSelected);

        }
        [Authorize(Roles = "Gerente")]
        [HttpGet]
        public async Task<IActionResult> SelectSandwichesForOffer(string SandwichName, double sandwichPrecio)
        {
            SelectSandwichesForOfferViewModel selectSandwiches = new SelectSandwichesForOfferViewModel();
            selectSandwiches.Sandwiches = await _context.Sandwich
                .Include(s => s.IngredienteSandwich).ThenInclude(isa => isa.Ingrediente)
                .Where(s => (s.SandwichName.Contains(SandwichName) || SandwichName == null) && (s.Precio <= sandwichPrecio || sandwichPrecio == 0.0))
                .OrderBy(s => s.SandwichName)
                .Where(s => !(s is SandwCreado))
                .Select(s => new SandwichForOfferViewModel(s)).ToListAsync();

            return View(selectSandwiches);
        }
        [Authorize(Roles = "Gerente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectSandwichesForOffer(SelectedSandwichesForOfferViewModel selectedSandwich)
        {
            if (selectedSandwich.IdsToAdd != null)
            {
                return RedirectToAction("Create", "Ofertas", selectedSandwich);
            }
            //a message error will be shown to the customer in case no movies are selected
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos un Sándwich");

            //the View SelectMoviesForPurchase will be shown again
            return await SelectSandwichesForOffer(selectedSandwich.SandwichName, selectedSandwich.sandwichPrecio);

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
        [Authorize(Roles = "Gerente")]
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
        [Authorize(Roles = "Gerente")]
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
        [Authorize(Roles = "Gerente")]
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
        [Authorize(Roles = "Gerente")]
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
        [Authorize(Roles = "Gerente")]
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
