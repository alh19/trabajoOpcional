using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.ProveedorViewModels;

namespace Sandwich2Go.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string SearchString)
        {
            var proveedores = _context.Proveedor.
                    Where(s => s.Nombre.Contains(SearchString)).
                    OrderBy(i => i.Cif);
            return View(await proveedores.ToListAsync());
        }

        /* ANTIGUA CLASE INDEX. SEGÚN DIAPOSITIVAS DE MOODLE
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
        */

        [HttpGet]
        public IActionResult SelectProveedoresForPurchase(string proveedorCif, string nombreProveedorSelected)
        {
            SelectProveedoresViewModel selectProveedores = new SelectProveedoresViewModel();
            
            //Muestra un listado con el nombre de los proveedores disponibles
            selectProveedores.Nombres = 
                new SelectList(_context.Proveedor.Select(g => g.Nombre).ToList());

            //Filtra por nombre y/o cif
            selectProveedores.Proveedores = _context.Proveedor
                //.Include(m => m.Nombre)
                .Where(prov => (prov.Nombre.Contains(nombreProveedorSelected) || nombreProveedorSelected==null)
                && ((prov.Cif.Contains(proveedorCif) || proveedorCif==null)));

            selectProveedores.Proveedores = selectProveedores.Proveedores.ToList();

            return View(selectProveedores);

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

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectProveedoresForPurchase(SelectedProveedoresForPurchaseViewModel selectedProveedor)
        {
            //Si el usuario ha seleccionado algún proveedor, entonces crearemos la compra.
            //Para ello llamaremos al método de acción Create (GET) de Purchase

            if (selectedProveedor.IdsToAdd != null)
            {
                //Redirecciona a la página ingrediente
                Ingrediente ingr = new Ingrediente();
                return RedirectToAction("Create", "Ingredientes", ingr);
            }
            //Si el usuario no ha seleccionado ninguna película, le informaremos y
            //se vuelve a generar el ViewModel
            ModelState.AddModelError(string.Empty, "You must select at least one movie");

            //the View SelectMoviesForPurchase will be shown again
            return SelectProveedoresForPurchase((selectedProveedor.proveedorCif), 
                selectedProveedor.nombreProveedorSelected);
        }

       

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor
                .FirstOrDefaultAsync(m => m.Id == id);
            
            /*
            var proveedor = await _context.Proveedor
                .Include(p => p.IngrProv)
                .ThenInclude(p => p.Ingrediente)
                .FirstAsync(p => p.Id.Equals(id));
            */

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
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cif,Nombre,Direccion")] Proveedor proveedor)
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
        public async Task<IActionResult> Delete(int? id)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedor.FindAsync(id);
            _context.Proveedor.Remove(proveedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedor.Any(e => e.Id == id);
        }
    }
}
