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
        public async Task<IActionResult> DetailsCreado(int? id)
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

            return View(new PedidoDetailsSandwichPersonalizadoViewModel(pedido));
        }
        // GET: Pedidos/Create
        public async Task<IActionResult> CreateSandwichPersonalizado(SelectedIngredientesForPurchaseViewModel
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
            SandwichPedido sandwichPedido;
            SandwCreado sandwCreado=null;
            IngredienteSandwich ingredienteSandwich;
            Cliente cliente;
            Pedido pedido;
            
          //  PedidoCreateSandwichPersonalizadoViewModel pedidoViewModel1 = pedidoViewModel;
        //    pedido.sandwichesPedidos = new List<SandwichPedido>();
            cliente = await _context.Users.OfType<Cliente>().FirstOrDefaultAsync<Cliente>(c => c.UserName.Equals(User.Identity.Name));
            double precioCompra = 0;
            double preciosand = 0;
            IList<IngredienteSandwich> ingsand = new List<IngredienteSandwich>();
            IList<SandwichPedido> sandped = new List<SandwichPedido>();
            IList<SandwichPedido> sp=new List<SandwichPedido>();

            if (ModelState.IsValid)
            {
                foreach (IngredientePedidoViewModel ingredienteP in pedidoViewModel.ingPedidos)
                {
                    bool puedePedir = true;
                    ingrediente = await _context.Ingrediente
                    .FirstOrDefaultAsync<Ingrediente>(s => (s.Id == ingredienteP.Id) && (s.Stock>=pedidoViewModel.Cantidad * ingredienteP.cantidad));
                    if (ingrediente == null)
                        ModelState.AddModelError("", $"No hay cantidad suficiente del ingrediente seleccionado {ingredienteP.Nombre}");
                    else
                    {
                        preciosand += ingredienteP.cantidad * ingredienteP.PrecioUnitario;
                        ingredienteSandwich = new IngredienteSandwich()
                        {
                            Sandwich = sandwCreado,
                            Ingrediente = ingrediente,
                            //SandwichId = sandwCreado.Id,
                            //IngredienteId = ingrediente.Id,

                        };
                        ingsand.Add(ingredienteSandwich);
                        ingrediente.Stock -= pedidoViewModel.Cantidad * ingredienteP.cantidad;
                        precioCompra += ingredienteP.PrecioUnitario * pedidoViewModel.Cantidad;
                    }
                }
            }
            if (ModelState.ErrorCount > 0)
            {
                pedidoViewModel.Name = cliente.Nombre;
                pedidoViewModel.Apellido = cliente.Apellido;
                return View(pedidoViewModel);
            }
            sandwCreado = new SandwCreado(pedidoViewModel.DireccionEntrega, preciosand, "", sandped, ingsand, null);

            pedido = new Pedido()
            {
                Nombre = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString(),
                Fecha = DateTime.Now,
                Preciototal = (int)precioCompra,
                Cantidad = 1,
                Direccion=pedidoViewModel.DireccionEntrega,
                Descripcion = "Pedido con el sandwich " + sandwCreado.SandwichName,
                SandwCreado=sandwCreado,
                sandwichesPedidos=sp,
                Cliente=cliente,
                
            };
            

            sandwichPedido = new SandwichPedido()
            {
                Sandwich = sandwCreado,
                //SandwichId = sandwCreado.Id,
                Pedido = pedido,
                //PedidoId = pedido.Id,
                Cantidad = pedido.Cantidad
            };
            
            sandped.Add(sandwichPedido);
            for (int i = 0; i < pedidoViewModel.Cantidad; i++)
            {
                sp.Add(sandwichPedido);
            }
            
            






            if (pedidoViewModel.MetodoPago == "Tarjeta")
                {
                    pedido.MetodoDePago = new Tarjeta()
                    {
                        Numero = long.Parse(pedidoViewModel.NumeroTarjetaCredito),
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

            


                 _context.Add(pedido);

                _context.SaveChanges();

                return RedirectToAction("DetailsCreado", new { id = pedido.Id });
            
        }
            //}

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

