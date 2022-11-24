using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.PedidoViewModels;
using Sandwich2Go.Models.SandwichViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sandwich2Go.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            Cliente cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(c => c.UserName.Equals(User.Identity.Name));
            return View(await _context.Pedido.Where(p => p.Cliente.Id.Equals(cliente.Id)).ToListAsync());
        }
        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Cliente cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(c => c.UserName.Equals(User.Identity.Name));
            var pedido = await _context.Pedido
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null || !pedido.Cliente.Id.Equals(cliente.Id))
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Create(SelectedSandwichesForPurchaseViewModel selectedSandwiches)
        {
            PedidoSandwichCreateViewModel pedido = new PedidoSandwichCreateViewModel();
            pedido.sandwichesPedidos = new List<SandwichPedidoViewModel>();

            if (selectedSandwiches.IdsToAdd == null)
            {
                ModelState.AddModelError("SandwichNoSelected", "Debes elegir al menos un sándwich para crear un pedido.");
            }
            else
            {
                IList<string> idsSandwiches = selectedSandwiches.IdsToAdd.ToList();
                pedido.sandwichesPedidos = await _context.Sandwich
                    .Include(s => s.IngredienteSandwich).ThenInclude(ins => ins.Ingrediente).ThenInclude(ing => ing.AlergSandws).ThenInclude(als => als.Alergeno)
                    .Include(s => s.OfertaSandwich).ThenInclude(os => os.Oferta)
                    .Where(s => idsSandwiches.Contains(s.Id.ToString()))
                    .Select(s => new SandwichPedidoViewModel(s))
                    .ToListAsync();
            }

            Cliente cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(c => c.UserName.Equals(User.Identity.Name));

            pedido.Name = cliente.Nombre;
            pedido.Apellido = cliente.Apellido;
            pedido.precioTotal();
            return View(pedido);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PedidoSandwichCreateViewModel pedidoViewModel)
        {
            Sandwich sandwich;
            SandwichPedido sandwichPedido;
            Cliente cliente;
            Pedido pedido = new();
            pedido.Preciototal = 0;
            pedido.sandwichesPedidos = new List<SandwichPedido>();
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(c => c.UserName.Equals(User.Identity.Name));
            
            if(ModelState.IsValid)
            {
                foreach (SandwichPedidoViewModel sandwichP in pedidoViewModel.sandwichesPedidos)
                {
                    sandwich = await _context.Sandwich.FirstOrDefaultAsync<Sandwich>(s => s.Id == sandwichP.Id);
                }
            }

            return RedirectToAction("Details", new {id = pedido.Id});
        }

        [Authorize(Roles = "Gerente")]
        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Gerente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Fecha,Preciototal,Direccion,Descripcion,Cantidad")] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
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
            return View(pedido);
        }
        [Authorize(Roles = "Gerente")]
        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Gerente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);
            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedido.Any(e => e.Id == id);
        }
    }
}
