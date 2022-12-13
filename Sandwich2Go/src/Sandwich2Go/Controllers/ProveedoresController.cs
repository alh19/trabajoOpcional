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
using Sandwich2Go.Models.IngredienteViewModels;
using Sandwich2Go.Models.ProveedorViewModels;

namespace Sandwich2Go.Controllers
{
    [Authorize]
    public class ProveedoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Gerente")]
        [HttpGet]
        public IActionResult SelectProveedoresForPurchase(string? proveedorNombreSelected)
        {

            SelectProveedoresForPurchaseViewModel selectProveedores = new SelectProveedoresForPurchaseViewModel();

            selectProveedores.Proveedores = _context.Proveedor
            .Select((m) => m)
            .Where(prov => prov.Nombre.Equals(proveedorNombreSelected) || proveedorNombreSelected == null);

            return View(selectProveedores);
        }

        [Authorize(Roles = "Gerente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectProveedoresForPurchase(SelectedProveedoresForPurchaseViewModel
        selectedProveedores)
        {

            if (selectedProveedores.IdsToAdd != null)
            {
                //La siguiente pantalla es la selección de ingredientes por parte de los proveedores
                return RedirectToAction("SelectIngrProvForPurchase", "Ingredientes", selectedProveedores);
            }

            ModelState.AddModelError(string.Empty, "You must select at least one supplier");

            return SelectProveedoresForPurchase(selectedProveedores.proveedorNombreSelected);
        }

        // GET: Proveedores
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proveedor.ToListAsync());
        }

        // GET: Proveedores/Details/5
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor
                //.FirstOrDefaultAsync(m => m.Id == id);
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: Proveedores/Create
        [Authorize(Roles = "Gerente")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Gerente")]
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
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id.Equals(null))
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
        [Authorize(Roles = "Gerente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cif,Nombre,Direccion")] Proveedor proveedor)
        {
            //if (id != proveedor.Id)
            if (!id.Equals(proveedor.Id))
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
                    //if (!ProveedorExists(int.Parse(proveedor.Id)))
                    if (!ProveedorExists(proveedor.Id))
                    //if (!ProveedorExists(proveedor.Id.ToString()))
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
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor
                //.FirstOrDefaultAsync(m => m.Id == id);
                .FirstOrDefaultAsync(m => m.Id.Equals(id));
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Gerente")]
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
            //return _context.Proveedor.Any(e => e.Id == id);
            return _context.Proveedor.Any(e => e.Id.Equals(id));
        }
    }
}
