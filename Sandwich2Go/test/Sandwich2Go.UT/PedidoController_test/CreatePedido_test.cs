using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.SandwichViewModels;
using Sandwich2Go.Models.PedidoViewModels;
//using Sandwich2Go.UT.ProveedoresController_test;
using Sandwich2Go.UT.SandwichControllers_test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Humanizer.In;
using Xunit.Sdk;

namespace Sandwich2Go.UT.PedidoController_test
{
    public class CreatePedido_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext pedidosHttpContext;

        public CreatePedido_test()
        {
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            UtilitiesForPedido.InitializeDbPedidosForTests(context);

            //Conexión de usuario
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("gregorio@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            pedidosHttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            pedidosHttpContext.User = identity;
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void CreatePedido_SandwichesNotSelected()
        {
            using (context)
            {
                //Arrange
                var controller = new PedidosController(context);

                controller.ControllerContext.HttpContext = pedidosHttpContext;
                Cliente cliente = Utilities.GetUsers(1, 1).First() as Cliente;
                var expectedViewModel = new PedidoSandwichCreateViewModel
                {
                    Name = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    IdCliente = "2",
                    necesitaCambio = false,
                    sandwichesPedidos = UtilitiesForPedido.GetSandwiches(0, 0).OrderBy(s => s.SandwichName).Select(s => new SandwichPedidoViewModel(s)).ToList(),
                };

                SelectedSandwichesForPurchaseViewModel selected = new SelectedSandwichesForPurchaseViewModel { IdsToAdd = null, sandwichAlergenoSelected = null, sandwichPrecio = "0" };

                //Act

                var result = controller.Create(selected);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);

                PedidoSandwichCreateViewModel model = viewResult.Model as PedidoSandwichCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                //Comprobamos igualdad entre ViewModels
                Assert.Equal(expectedViewModel, model);
                Assert.Equal("Debes elegir al menos un sándwich para crear un pedido.", error.ErrorMessage);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void CreatePedido_SandwichesSelected()
        {
            using (context)
            {
                //Arrange
                var controller = new PedidosController(context);

                controller.ControllerContext.HttpContext = pedidosHttpContext;
                Cliente cliente = Utilities.GetUsers(1, 1).First() as Cliente;
                var expectedViewModel = new PedidoSandwichCreateViewModel
                {
                    Name = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    IdCliente = "2",
                    necesitaCambio = false,
                    sandwichesPedidos = UtilitiesForPedido.GetSandwiches(0, 2).OrderBy(s => s.SandwichName).Select(s => new SandwichPedidoViewModel(s)).ToList(),
                };

                SelectedSandwichesForPurchaseViewModel selected = new SelectedSandwichesForPurchaseViewModel { IdsToAdd = new string[] { "1", "2" }, sandwichAlergenoSelected = null, sandwichPrecio = "0" };

                //Act

                var result = controller.Create(selected);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);

                PedidoSandwichCreateViewModel model = viewResult.Model as PedidoSandwichCreateViewModel;
                //Comprobamos igualdad entre ViewModels
                Assert.Equal(expectedViewModel, model);
            }
        }

        public static IEnumerable<object[]> TestCasesForCompraCreatePost_WithErrors()
        {
            UtilitiesForPedido.CrearDatos();
            Sandwich sandwich1 = UtilitiesForPedido.GetSandwiches(0, 1).First();
            Cliente cliente = Utilities.GetUsers(1, 1).First() as Cliente;

            //Primer error: Sándwich con cantidad inválida
            SandwichPedidoViewModel sp1 = new SandwichPedidoViewModel(sandwich1);
            sp1.cantidad = 0;
            //Valores input

            IList<SandwichPedidoViewModel> sandwichesPedidos1 = new SandwichPedidoViewModel[1]
            {
                sp1
            };

            PedidoSandwichCreateViewModel pedidoInput1 = new PedidoSandwichCreateViewModel();

            pedidoInput1.sandwichesPedidos.Add(sandwichesPedidos1[0]);
            pedidoInput1.IdCliente = cliente.Id;
            pedidoInput1.Name = cliente.Nombre;
            pedidoInput1.Apellido = cliente.Apellido;

            //Valores esperados
            PedidoSandwichCreateViewModel pedidoEsperado1 = new PedidoSandwichCreateViewModel();

            pedidoEsperado1.sandwichesPedidos.Add(sandwichesPedidos1[0]);
            pedidoEsperado1.IdCliente = cliente.Id;
            pedidoEsperado1.Name = cliente.Nombre;
            pedidoEsperado1.Apellido = cliente.Apellido;

            string errorEsperado1 = "El restaurante no puede preparar en estos momentos el sándwich Cubano, por favor, selecciona una cantidad distinta o no lo incluyas en el pedido.";

            //Segundo error: Sandwich sin ingredientes suficientes

            Sandwich sandwich2 = UtilitiesForPedido.GetSandwiches(0, 1).First();
            SandwichPedidoViewModel sp2 = new SandwichPedidoViewModel(sandwich2);
            sp2.cantidad = 200;//No hay cantidad suficiente de ingredientes para preparar los sándwiches
            //Valores input
            IList<SandwichPedidoViewModel> sandwichesPedidos2 = new SandwichPedidoViewModel[1]
            {
                sp2
            };

            PedidoSandwichCreateViewModel pedidoInput2 = new PedidoSandwichCreateViewModel();

            pedidoInput2.sandwichesPedidos.Add(sandwichesPedidos2[0]);
            pedidoInput2.IdCliente = cliente.Id;
            pedidoInput2.Name = cliente.Nombre;
            pedidoInput2.Apellido = cliente.Apellido;

            //Valores esperados
            PedidoSandwichCreateViewModel pedidoEsperado2 = new PedidoSandwichCreateViewModel();

            pedidoEsperado2.sandwichesPedidos.Add(sandwichesPedidos2[0]);
            pedidoEsperado2.IdCliente = cliente.Id;
            pedidoEsperado2.Name = cliente.Nombre;
            pedidoEsperado2.Apellido = cliente.Apellido;

            string errorEsperado2 = "El restaurante no puede preparar en estos momentos el sándwich Cubano, por favor, selecciona una cantidad distinta o no lo incluyas en el pedido.";

            var allTest = new List<object[]>
            {
                new object[]{pedidoInput1,pedidoEsperado1,errorEsperado1},
                new object[]{pedidoInput2,pedidoEsperado2,errorEsperado2}
            };
            UtilitiesForPedido.BorrarDatos();
            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesForCompraCreatePost_WithErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithErrors(PedidoSandwichCreateViewModel pedidoInput, PedidoSandwichCreateViewModel pedidoEsperado, string ErrorEsperado)
        {
            using (context)
            {
                //Arrange
                var controller = new PedidosController(context);
                controller.ControllerContext.HttpContext = pedidosHttpContext;

                //Act
                var result = controller.CreatePost(pedidoInput);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result.Result);
                PedidoSandwichCreateViewModel pedidoActual = viewResult.Model as PedidoSandwichCreateViewModel;

                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                Assert.Equal(pedidoEsperado, pedidoActual);
                Assert.Equal(ErrorEsperado, error.ErrorMessage);
            }
        }

        public static IEnumerable<object[]> TestCasesForCompraCreatePost_WithoutErrors()
        {
            
            Cliente cliente = Utilities.GetUsers(1, 1).First() as Cliente;
            IList<MetodoDePago> metodosDePago = UtilitiesForPedido.GetMetodosDePago(0, 2);
            IList<Sandwich> sandwiches = UtilitiesForPedido.GetSandwiches(0, 3);
            IList<Pedido> pedidos = UtilitiesForPedido.GetPedidos(0, 2);

            IList<int> cantidades1 = UtilitiesForPedido.GetIngredientes(0, 4).Select(i => i.Stock).ToList();
            IList<int> cantidades2 = UtilitiesForPedido.GetIngredientes(0, 4).Select(i => i.Stock).ToList();
            //Compra con tarjeta
            Pedido pedidoEsperado1 = pedidos[0];
            PedidoSandwichCreateViewModel pedidoCVM1 = new PedidoSandwichCreateViewModel
            {
                IdCliente = cliente.Id,
                Name = cliente.Nombre,
                Apellido = cliente.Apellido,
                DireccionEntrega = pedidoEsperado1.Direccion,
                PrecioTotal = pedidoEsperado1.Preciototal,
                MetodoPago = "Tarjeta",
                MesCad = (pedidoEsperado1.MetodoDePago as Tarjeta).MesCaducidad.ToString(),
                AnoCad = (pedidoEsperado1.MetodoDePago as Tarjeta).AnoCaducidad.ToString(),
                CCV = (pedidoEsperado1.MetodoDePago as Tarjeta).CCV.ToString(),
                NumeroTarjetaCredito = (pedidoEsperado1.MetodoDePago as Tarjeta).Numero.ToString(),
                sandwichesPedidos = pedidoEsperado1.sandwichesPedidos.Select(s => new SandwichPedidoViewModel(s.Sandwich)).ToList()
            };

            cantidades1[0] -= 1;
            cantidades1[1] -= 1;
            cantidades1[3] -= 1;

            pedidos[0].Fecha = DateTime.Now;
            pedidos[0].Descripcion = "Pedido con los sándwiches Cubano ";
            pedidos[0].Cliente = cliente;
            pedidos[0].Nombre = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();

            (pedidos[0].MetodoDePago as Tarjeta).Titular = cliente.Nombre + " " + cliente.Apellido;

            //Compra en efectivo
            Pedido pedidoEsperado2 = pedidos[1];
            PedidoSandwichCreateViewModel pedidoCVM2 = new PedidoSandwichCreateViewModel
            {
                IdCliente = cliente.Id,
                Name = cliente.Nombre,
                Apellido = cliente.Apellido,
                DireccionEntrega = pedidoEsperado2.Direccion,
                PrecioTotal = pedidoEsperado2.Preciototal,
                MetodoPago = "Efectivo",
                necesitaCambio = false,
                sandwichesPedidos = pedidoEsperado2.sandwichesPedidos.Select(s => new SandwichPedidoViewModel(s.Sandwich)).ToList()
            };
            cantidades2[0] -= 2;
            cantidades2[1] -= 2;
            cantidades2[2] -= 1;

            pedidos[1].Fecha = DateTime.Now;
            pedidos[1].Descripcion = "Pedido con los sándwiches Mixto Inglés ";
            pedidos[1].Cliente = cliente;
            pedidos[1].Nombre = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            pedidos[1].Id =1;
            (pedidos[1].MetodoDePago as Efectivo).NecesitasCambio = false;


            var allTest = new List<object[]>
            {
                new object[] {pedidoCVM1, pedidoEsperado1, cantidades1 },
                new object[] {pedidoCVM2, pedidoEsperado2, cantidades2 }
            };
            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesForCompraCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(PedidoSandwichCreateViewModel pedidoCVM, Pedido pedidoEsperado, List<int> cantidadesEsperadas)
        {
            using (context)
            {
                
                //Arrange
                var controller = new PedidosController(context);
                controller.ControllerContext.HttpContext = pedidosHttpContext;

                //Act
                var result = controller.CreatePost(pedidoCVM);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

                Assert.Equal("Details", viewResult.ActionName);

                Assert.Equal(pedidoEsperado.Id, viewResult.RouteValues.First().Value);

                var actualPedido = context.Pedido.Include(p => p.sandwichesPedidos).ThenInclude(sc => sc.Sandwich).ThenInclude(s => s.IngredienteSandwich).ThenInclude(isa => isa.Ingrediente).ThenInclude(i => i.AlergSandws).ThenInclude(als => als.Alergeno)
                    .Include(p => p.MetodoDePago)
                    .Include(p => p.sandwichesPedidos).ThenInclude(sp => sp.Sandwich).ThenInclude(s => s.OfertaSandwich).ThenInclude(os => os.Oferta)
                    .FirstOrDefault(p => p.Id == pedidoEsperado.Id);
                Assert.Equal(pedidoEsperado, actualPedido);

                List<int> cantidadesReales = context.Ingrediente.OrderBy(i => i.Id).Select(i => i.Stock).ToList();


                Assert.Equal(cantidadesEsperadas, cantidadesReales);
            }

        }
    }
}
