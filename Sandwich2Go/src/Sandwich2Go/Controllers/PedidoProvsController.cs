using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using Sandwich2Go.Models.IngredienteViewModels;
using Sandwich2Go.Models.PedidoProvViewModels;
using Efectivo = Sandwich2Go.Models.Efectivo;
using Gerente = Sandwich2Go.Models.Gerente;
using Ingrediente = Sandwich2Go.Models.Ingrediente;
using IngrPedProv = Sandwich2Go.Models.IngrPedProv;
using IngrProv = Sandwich2Go.Models.IngrProv;
using PedidoProv = Sandwich2Go.Models.PedidoProv;
using Proveedor = Sandwich2Go.Models.Proveedor;
using Tarjeta = Sandwich2Go.Models.Tarjeta;

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

            var x = id;


            var pedidoProv = await _context.PedidoProv
                .Include(p => p.IngrPedProv).ThenInclude(p => p.IngrProv)
                .ThenInclude(p => p.Proveedor).ThenInclude(p => p.IngrProv).ThenInclude(p => p.Ingrediente)
                .FirstOrDefaultAsync(m => m.Id == id);

            var ingrPedidoProv = pedidoProv.IngrPedProv.First().IngrProv;

            if (ingrPedidoProv == null || pedidoProv == null)
            {
                return NotFound();
            }

            PedidoProvDetailsViewModel pedidoFinalView = new PedidoProvDetailsViewModel(ingrPedidoProv);

            pedidoFinalView.PrecioTotal = pedidoProv.PrecioTotal;
            pedidoFinalView.Ingredientes = pedidoProv.IngrPedProv.Select(p => p.IngrProv).Select(p => p.Ingrediente).ToList();

            return View(pedidoFinalView);
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
                if (selingrprov.IdsToAdd == null)
                {
                    ModelState.AddModelError("IngrNoSelected", "Tienes que seleccionar al menos un ingrediente para realizar el pedido");
                }
                else
                {
                    pedidoprov.ingredientesPedProv = await _context.Ingrediente
                        .Include(p => p.IngrProv)
                        .Select(p => new PedidoProvCreateViewModel.IngrPedProvViewModel()
                        {
                            Id = p.Id,
                            NombreIngrediente = p.Nombre,
                            PrecioUnitario = p.PrecioUnitario,
                            Stock = p.Stock,
                            //IdsIngrProv = p.IngrProv.Select(s => s.Id ).ToList(),

                        })
                        .Where(p => selingrprov.IdsToAdd.Contains(p.Id.ToString())).ToListAsync();



                    proveedor = await _context.Proveedor
                        .Where(n => selingrprov.IdProveedor == n.Id)
                        .Select(m => m).FirstAsync();
                }

                pedidoprov.IdProveedor = selingrprov.IdProveedor;
                pedidoprov.Cif = proveedor.Cif;
                pedidoprov.NombreProveedor = proveedor.Nombre;
                pedidoprov.Direccion = proveedor.Direccion;
                pedidoprov.FechaPedido = System.DateTime.Now;

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
        public async Task<IActionResult> Create(PedidoProvCreateViewModel pedidoprovvm)
        {
            Gerente gerente = await _context.Users.OfType<Gerente>().FirstOrDefaultAsync<Gerente>(
                g => g.UserName.Equals(User.Identity.Name));
            Ingrediente ingrediente;
            Proveedor proveedor;
            PedidoProv pedidoFinal = new();
            IngrProv detallesPedido = new();
            pedidoFinal.IngrPedProv = new List<IngrPedProv>();
            //pedidoFinal.
            IngrPedProv ingrPedProvAux;

            if (ModelState.IsValid)
            {
                //No son tipos convertibles
                foreach (var ingrpedprov in pedidoprovvm.ingredientesPedProv)
                {
                    ingrediente = await _context.Ingrediente.FirstOrDefaultAsync
                        (p => p.Id == ingrpedprov.Id);

                    detallesPedido = await _context.IngrProv.FirstOrDefaultAsync<IngrProv>
                        (m => m.Id == ingrpedprov.Id);

                    //detallesPedido = await _context.IngrProv
                    //    .Where(p => p.Id == ingrpedprov.Id)
                    //    .Select(p => p).ToListAsync();

                    if (ingrpedprov.Cantidad <= 0)
                    {
                        ModelState.AddModelError("", $"Debes seleccionar una cantidad de stock " +
                            $"mayor que 0 de {ingrediente.Nombre}");
                    }

                    //Si el stock es correcto
                    if (ingrpedprov.Cantidad > ingrediente.Stock)
                    {
                        ModelState.AddModelError("", $"Has seleccionado una cantidad de stock " +
                            $"mayor a la que el proveedor tiene de {ingrediente.Nombre}");
                    }
                    else
                    {

                        if (ingrpedprov.Cantidad > 0)
                        {
                            ingrediente.Stock += ingrpedprov.Cantidad;
                            //IngrProv ingrprovax = detallesPedido.Where(p => p.Id == pedidoprovvm.IdProveedor).Select(p => p).First(),
                            ingrPedProvAux = new IngrPedProv
                            //(ingrpedprov.Id, ingrpedprov.Cantidad, pedidoFinal, pedidoFinal.Id,
                            //    ingrprovax, pedidoprovvm.IdProveedor);
                            {
                                //Id = ingrpedprov.Id,

                                Cantidad = ingrpedprov.Cantidad,
                                PedidoProv = pedidoFinal,
                                //PedidoProvId = pedidoFinal.Id,
                                //IngrProv = detallesPedido.Where(p => p.Id == pedidoprovvm.IdProveedor).Select(p => p).First(),
                                IngrProv = detallesPedido,
                                //IngrProvId = pedidoprovvm.IdProveedor,
                            };
                            ingrPedProvAux.IngrProv.IngredienteId = ingrediente.Id;
                            pedidoFinal.PrecioTotal += ingrpedprov.Cantidad * ingrediente.PrecioUnitario;
                            pedidoFinal.IngrPedProv.Add(ingrPedProvAux);
                            Console.WriteLine(pedidoFinal.IngrPedProv.ToString());
                        }

                    }
                }
            }
            //Si hay error volvemos a la vista
            if (ModelState.ErrorCount > 0)
            {
                return View(pedidoprovvm);
            }

            if (pedidoprovvm.MetodoPago == "Tarjeta")
            {
                pedidoFinal.MetodoDePago = new Tarjeta()
                {
                    Numero = long.Parse(pedidoprovvm.NumeroTarjetaCredito),
                    CCV = int.Parse(pedidoprovvm.CCV),
                    MesCaducidad = int.Parse(pedidoprovvm.MesCad),
                    AnoCaducidad = int.Parse(pedidoprovvm.AnoCad)
                };
            }
            else
            {
                pedidoFinal.MetodoDePago = new Efectivo()
                {
                    NecesitasCambio = pedidoprovvm.necesitaCambio
                };
            }


            /*pedidoprovvm.IdIngrProv await _context.Users.OfType<Gerente>().FirstOrDefaultAsync<Gerente>(
            g => g.UserName.Equals(User.Identity.Name));*/
            pedidoFinal.DireccionEnvio = "Calle Restaurante Sandwich";
            pedidoFinal.Gerente = gerente;
            pedidoFinal.FechaPedido = pedidoprovvm.FechaPedido;



            _context.Add(pedidoFinal);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = pedidoFinal.Id });
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
