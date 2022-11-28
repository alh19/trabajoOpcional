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
                .Include(p => p.MetodoDePago)
                .Include(p => p.sandwichesPedidos).ThenInclude(sp => sp.Sandwich).ThenInclude(s => s.IngredienteSandwich).ThenInclude(isa => isa.Ingrediente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null || !pedido.Cliente.Id.Equals(cliente.Id))
            {
                return NotFound();
            }

            return View(new PedidoDetailsViewModel (pedido));
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
            pedido.IdCliente = cliente.Id;
            pedido.precioTotal();
            return View(pedido);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PedidoSandwichCreateViewModel pedidoViewModel)
        {
            Sandwich sandwich;
            SandwichPedido sandwichPedido;
            Cliente cliente;
            Pedido pedido = new();
            pedido.Preciototal = 0;
            PedidoSandwichCreateViewModel pedidoViewModel1 = pedidoViewModel;
            pedido.sandwichesPedidos = new List<SandwichPedido>();
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(c => c.UserName.Equals(User.Identity.Name));
            double precioCompra = 0;
            string sandws = "";

            if (ModelState.IsValid)
            {
                foreach (SandwichPedidoViewModel sandwichP in pedidoViewModel.sandwichesPedidos)
                {
                    bool puedePedir = true;
                    sandwich = await _context.Sandwich
                    .Include(s => s.IngredienteSandwich).ThenInclude(insa => insa.Ingrediente)
                    .FirstOrDefaultAsync<Sandwich>(s => s.Id == sandwichP.Id);
                 //   pedidoViewModel1.sandwichesPedidos.Add(sandwichP);
                    foreach (IngredienteSandwich insa in sandwich.IngredienteSandwich)
                    {
                        if(insa.Ingrediente.Stock < insa.Cantidad*sandwichP.cantidad)
                        {
                            puedePedir = false;
                            ModelState.AddModelError("",$"No hay suficiente/s {insa.Ingrediente.Nombre} para el sándwich {sandwich.SandwichName}, por favor, selecciona menos sándwiches o no lo incluyas en el pedido.");
                        }
                        else
                        {
                            insa.Ingrediente.Stock = insa.Ingrediente.Stock - (insa.Cantidad * sandwichP.cantidad);
                        }
                    }
                    if (puedePedir && sandwichP.cantidad>0)
                    {
                        sandwichPedido = new SandwichPedido()
                        {
                            Sandwich = sandwich,
                            SandwichId = sandwich.Id,
                            Pedido = pedido,
                            PedidoId = pedido.Id,
                            Cantidad = sandwichP.cantidad
                        };
                        precioCompra += sandwichP.PrecioCompra;
                        sandws += sandwichP.NombreSandwich + " ";
                        pedido.sandwichesPedidos.Add(sandwichPedido);
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                pedidoViewModel1.Name = cliente.Nombre;
                pedidoViewModel1.Apellido = cliente.Apellido;
                return View(pedidoViewModel1);
            }
            pedido.Cliente = cliente;
            pedido.Fecha = DateTime.Now;

            if(pedidoViewModel.MetodoPago == "Tarjeta")
            {
                pedido.MetodoDePago = new Tarjeta()
                {
                    Numero = int.Parse(pedidoViewModel.NumeroTarjetaCredito),
                    CCV = int.Parse(pedidoViewModel.CCV),
                    MesCaducidad = int.Parse(pedidoViewModel.MesCad),
                    AnoCaducidad = int.Parse(pedidoViewModel.AnoCad),
                    Titular = cliente.Nombre + " " + cliente.Apellido
                };
            }
            else
            {
                pedido.MetodoDePago = new Efectivo()
                {
                    NecesitasCambio = pedidoViewModel.necesitaCambio
                };
            }

            pedido.Cantidad = 1;
            pedido.Direccion = pedidoViewModel.DireccionEntrega;
            pedido.Preciototal = precioCompra;
            pedido.Descripcion = "Pedido del dia "+ DateTime.Now.ToString()+" con los sándwiches "+sandws;
            pedido.Nombre = DateTime.Now.ToString() + " " + pedido.Direccion;

            _context.Add(pedido);

            await _context.SaveChangesAsync();

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
