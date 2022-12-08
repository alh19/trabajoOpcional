using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Design;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.IngredienteViewModels;
using Sandwich2Go.Models.PedidoProvViewModels;
using PedidoProv = Sandwich2Go.Models.PedidoProv;
using Proveedor = Sandwich2Go.Models.Proveedor;

namespace Sandwich2Go.Controllers
{
    public class PedidoProvsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidoProvsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PedidoProvs
        public async Task<IActionResult> Index()
        {
            return View(await _context.PedidoProv.ToListAsync());
        }

        // GET: PedidoProvs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoProv = await _context.PedidoProv
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoProv == null)
            {
                return NotFound();
            }

            return View(pedidoProv);
        }

        [Authorize(Roles = "Gerente")]
        // GET: PedidoProvs/Create
        public async Task<IActionResult> Create(SelectedIngrProvForPurchaseViewModel selingrprov)
        {
            Proveedor proveedor = new();

            PedidoProvCreateViewModel pedidoprov = new PedidoProvCreateViewModel();
            //pedidoprov.NombreProveedor = "Alberto";
            if (selingrprov.IdProveedor != 0 || selingrprov.IdsToAdd != null)
            {
                if(selingrprov.IdsToAdd == null)
                {
                    ModelState.AddModelError("IngrNoSelected", "Tienes que seleccionar al menos un ingrediente para realizar el pedido");
                }
                else
                {
                    pedidoprov.ingredientesPedProv = await _context.Ingrediente
                        .Select(p => new PedidoProvCreateViewModel.IngrPedProvViewModel()
                        {
                            Id = p.Id,
                            NombreIngrediente = p.Nombre,
                            PrecioUnitario = p.PrecioUnitario,
                            Stock = p.Stock

                        })
                        .Where(p => selingrprov.IdsToAdd.Contains(p.Id.ToString())).ToListAsync();

                   

                    proveedor = await _context.Proveedor
                        .Where(n => selingrprov.IdProveedor == n.Id )
                        .Select(m => m).FirstAsync();
                }
            pedidoprov.Id = selingrprov.IdProveedor;
            pedidoprov.Cif = proveedor.Cif;
            pedidoprov.NombreProveedor = proveedor.Nombre;
            pedidoprov.Direccion = proveedor.Direccion;

            return View(pedidoprov);
            
            }
            else
            {
                ModelState.AddModelError("IngrNoSelected", "Deberías seleccionar al menos un ingrediente");
            }
            PedidoProvCreateViewModel pedidoprovcreatevm = new PedidoProvCreateViewModel();
            pedidoprovcreatevm.NombreProveedor = proveedor.Nombre;
            pedidoprovcreatevm.Cif = proveedor.Cif;
            pedidoprovcreatevm.Direccion = proveedor.Direccion;
            pedidoprovcreatevm.Id = proveedor.Id;

            return View(pedidoprovcreatevm);

        }

        // POST: PedidoProvs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrecioTotal,DireccionEnvio,FechaPedido")] PedidoProv pedidoProv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedidoProv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pedidoProv);
        }

        // GET: PedidoProvs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoProv = await _context.PedidoProv.FindAsync(id);
            if (pedidoProv == null)
            {
                return NotFound();
            }
            return View(pedidoProv);
        }

        // POST: PedidoProvs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrecioTotal,DireccionEnvio,FechaPedido")] PedidoProv pedidoProv)
        {
            if (id != pedidoProv.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidoProv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoProvExists(pedidoProv.Id))
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
            return View(pedidoProv);
        }

        // GET: PedidoProvs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoProv = await _context.PedidoProv
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidoProv == null)
            {
                return NotFound();
            }

            return View(pedidoProv);
        }

        // POST: PedidoProvs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidoProv = await _context.PedidoProv.FindAsync(id);
            _context.PedidoProv.Remove(pedidoProv);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoProvExists(int id)
        {
            return _context.PedidoProv.Any(e => e.Id == id);
        }
    }
}
