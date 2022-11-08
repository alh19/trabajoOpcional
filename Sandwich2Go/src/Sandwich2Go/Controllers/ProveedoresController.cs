using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.SelectProveedoresViewModel;
using Sandwich2Go.Models.SandwichViewModels;

namespace Sandwich2Go.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SelectProveedorForPedidoProveedor(double proveedorCif, string proveedorNombreSelected)
        {
            SelectProveedoresViewModel selectProveedores = new SelectProveedoresViewModel();
            
            selectProveedores.Nombres = 
                new SelectList(_context.Proveedor.Select(g => g.Nombre).ToList());

            selectProveedores.Proveedores =
                _context.Proveedor.Include(m => m.Cif)
                .Where(cif => cif.Cif.Contains((Char) proveedorCif) || proveedorCif.Equals(null));
            /*
            selectProveedores.Proveedores = _context.Proveedor
                .Include(s => s.IngrProv).ThenInclude(isa => isa.Ingrediente).ThenInclude(i => i.Nombre)
                .Where(s => (s.IngrProv
                    .Where(i => i.Ingrediente.IngrProv.Equals(proveedorNombreSelected)
                    .Any())
                .Count() == 0 || proveedorNombreSelected == null) && (s.Precio <= sandwichPrecio || sandwichPrecio == 0)).ToList();

                .Include(m => m.Genre) //join table Movie and table Genre
                .Where(movie => movie.QuantityForPurchase > 0 // where clause
                && (movie.Title.Contains(movieTitle) || movieTitle == null)
                && (movie.Genre.Name.Contains(movieGenreSelected) || movieGenreSelected == null));
            
            selectProveedores.Movies = selectMovies.Movies.ToList();

            selectProveedores.Proveedores = _context.Proveedor
                .Include(s => s.IngrProv).ThenInclude(isa => isa.Ingrediente).ThenInclude(i => i.Id)
                .Include(s => s.IngrProv).ThenInclude(isa => isa.Ingrediente).ThenInclude(i => i.Nombre)
                .Include(s => s.IngrProv).ThenInclude(isa => isa.Ingrediente).ThenInclude(i => i.Stock)
                .Where(ped => ped.IngrProv);
            */

            return View(selectProveedores);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectProveedorForPedidoProveedor(SelectedSandwichesForPurchaseViewModel selectedSandwich)
        {
            if (selectedSandwich.IdsToAdd != null)
            {

                return RedirectToAction("Create", "Pedido", selectedSandwich);
            }
            //a message error will be shown to the customer in case no movies are selected
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos un Sándwich");

            //the View SelectMoviesForPurchase will be shown again
            return SelectProveedorForPurchase(double.Parse(selectedSandwich.sandwichPrecio), selectedSandwich.sandwichAlergenoSelected);

        }

        // GET: Proveedores
        public async Task<IActionResult> Index(string SearchString)
        {
            //Se filtra por Nombre de proveedor y se ordena por CIF
            if (!String.IsNullOrEmpty(SearchString))
            {
                var proveedores = _context.Proveedor.
                    Where(s => s.Nombre.Contains(SearchString)).
                    OrderBy(i => i.Cif);
                return View(await proveedores.ToListAsync());
            }
            else
            {
                return View(await _context.Proveedor.
                    OrderBy(m => m.Cif).ToListAsync());
            }
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var proveedor = await _context.Proveedor
                .FirstOrDefaultAsync(m => m.Id == id);
            */

            //Muestra todos los detalles de los proveedores
            var proveedor = await _context.Proveedor
                .Include(p => p.IngrProv)
                .ThenInclude(p => p.Ingrediente)
                .FirstAsync(p => p.Id == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            //Al invocarse se realiza una llamada a la vista
            return View();
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cif,Nombre,Direccion")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Cif,Nombre,Direccion")] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.Id))
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
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var proveedor = await _context.Proveedor.FindAsync(id);
            _context.Proveedor.Remove(proveedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(string id)
        {
            return _context.Proveedor.Any(e => e.Id == id);
        }
    }
}
