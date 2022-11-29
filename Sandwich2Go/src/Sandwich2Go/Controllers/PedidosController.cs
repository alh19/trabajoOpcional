using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.IngredienteViewModels;
using Sandwich2Go.Models.PedidoSandwichPersonalizadoViewModels;
using Sandwich2Go.Models.SandwichViewModels;

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
        [Authorize(Roles = "Gerente")]
        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pedido.ToListAsync());
        }
        [Authorize(Roles = "Cliente")]
        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
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
        // GET: Pedidos/Create
        public async Task<IActionResult> CreateSandwichPersonalizadoAsync(SelectedIngredientesForPurchaseViewModel
        selectedIngredientes)
        {

            PedidoCreateSandwichPersonalizadoViewModel pedido = new PedidoCreateSandwichPersonalizadoViewModel();
            pedido.ingPedidos = new List<IngredientePedidoViewModel>();

            if (selectedIngredientes.IdsToAdd == null)
            {
                ModelState.AddModelError("IngredientNoSelected", "Debes elegir al menos un ingrediente para crear un pedido.");
            }
            else
            {
                IList<string> idsIng = selectedIngredientes.IdsToAdd.ToList();
                pedido.ingPedidos = await _context.Ingrediente
                    .Include(ing => ing.AlergSandws).ThenInclude(als => als.Alergeno)
                    .Where(s => idsIng.Contains(s.Id.ToString()))
                    .Select(s => new IngredientePedidoViewModel(s))
                    .ToListAsync();
            }

            Cliente cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(c => c.UserName.Equals(User.Identity.Name));
            pedido.Name = cliente.Nombre;
            pedido.Apellido = cliente.Apellido;
            pedido.IdCliente = cliente.Id;
            return View(pedido);
        }
        // POST: Pedidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSandwichPersonalizado(PedidoCreateSandwichPersonalizadoViewModel pedidoViewModel)
        {
            Ingrediente ingrediente;
            SandwCreado sandwichCreado;
            Cliente cliente;
            Pedido pedido = new();
            pedido.Preciototal = 0;
            PedidoCreateSandwichPersonalizadoViewModel pedidoViewModel1 = pedidoViewModel;
           
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(c => c.UserName.Equals(User.Identity.Name));
            double precioCompra = 0;
            string ings = "";

            if (ModelState.IsValid)
            {
                foreach (IngredientePedidoViewModel ingredienteP in pedidoViewModel.ingPedidos)
                {
                    bool puedePedir = true;
                    ingrediente = await _context.Ingrediente
                    .Include(s => s.AlergSandws).ThenInclude(insa => insa.Alergeno)
                    .FirstOrDefaultAsync<Ingrediente>(s => s.Id == ingredienteP.Id);
                    foreach (Ingrediente insa in pedidoViewModel.ingPedidos)
                    {
                        if (insa.Ingrediente.Stock < insa.Cantidad * ingredienteP.cantidad)
                        {
                            puedePedir = false;
                        }
                        else
                        {
                            insa.Ingrediente.Stock = insa.Ingrediente.Stock - (insa.Cantidad * ingredienteP.cantidad);
                        }
                    }
                    if (puedePedir && ingredienteP.cantidad > 0)
                    {
                        ingredientePedido = new SandwichCreado()
                        {
                            Ingrediente = ingrediente,
                            IngredienteId = ingrediente.Id,
                            Pedido = pedido,
                            PedidoId = pedido.Id,
                            Cantidad = ingredienteP.cantidad
                        };
                       
                        sandws += sandwichP.NombreSandwich + " ";
                        pedido.sandwichesPedidos.Add(sandwichPedido);
                    }
                    else
                    {
                        ModelState.AddModelError("", $"El restaurante no puede preparar en estos momentos el sándwich {ingrediente.Nombre}, por favor, selecciona una cantidad distinta o no lo incluyas en el pedido.");
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

            if (pedidoViewModel.MetodoPago == "Tarjeta")
            {
                pedido.MetodoDePago = new Tarjeta()
                {
                    Numero = (int)long.Parse(pedidoViewModel.NumeroTarjetaCredito),
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
            pedido.Descripcion = "Pedido con los ingredientes " + ings;
            pedido.Nombre = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();

            _context.Add(pedido);

            _context.SaveChanges();

            return RedirectToAction("Details", new { id = pedido.Id });
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
