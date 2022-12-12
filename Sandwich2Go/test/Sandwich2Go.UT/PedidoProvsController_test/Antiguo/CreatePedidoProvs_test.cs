
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models.IngredienteViewModels;
using Sandwich2Go.Models.PedidoProvViewModels;
using Sandwich2Go.UT.IngredientesController_test;
using Ingrediente = Sandwich2Go.Models.Ingrediente;
using IngrPedProvViewModel = Sandwich2Go.Models.IngrPedProv;
using IngrPedProvs = Sandwich2Go.Models.IngrPedProv;
using IngrPedProv = Sandwich2Go.Models.IngrPedProv;
using Proveedor = Sandwich2Go.Models.Proveedor;
using Gerente = Sandwich2Go.Models.Gerente;
using PedidoProv = Sandwich2Go.Models.PedidoProv;
using MetodoDePago = Sandwich2Go.Models.MetodoDePago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Sandwich2Go.UT.SandwichControllers_test;

namespace Sandwich2Go.UT.PedidoProvsController_test.Antiguo
{
    public class CreatePedidoProvs_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        DefaultHttpContext httpContext;
        static Gerente gerente;
        Proveedor proveedor;
        IList<IngrPedProv> ingrPedProv;

        public CreatePedidoProvs_test()
        {
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            gerente = context.Users.First() as Gerente;

            //Conexión de usuario
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            httpContext = new DefaultHttpContext()
            {
                User = identity
            };
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithSelectedIngrProv()
        {
            // Arrange
            var controller = new PedidoProvsController(context);
            //simulate user's connection
            controller.ControllerContext.HttpContext = httpContext;

            var IdProveedor = new SelectList(UtilitiesForIngredientes.GetProveedores(0, 1).Select(g => g.Id)).ToList().First().Text;
            var ingrNombre = new SelectList(UtilitiesForIngredientes.GetIngredientes(0, 1).Select(g => g.Nombre)).ToList().First().Text;
            var ingrPrecio = new SelectList(UtilitiesForIngredientes.GetIngredientes(0, 1).Select(g => g.PrecioUnitario)).ToList().First().Text;
            var ingrStock = new SelectList(UtilitiesForIngredientes.GetIngredientes(0, 1).Select(g => g.Stock)).ToList().First().Text;
            var IdIngrProv = new SelectList(UtilitiesForIngredientes.GetIngrProv(0, 1).Select(g => g.Id)).ToList().First().Text;

            SelectedIngrProvForPurchaseViewModel ingrProvVM = new()
            {
                IdsToAdd = new string[1] { "1" }
            };

            //new Proveedor { Id = 1, Nombre = "Alberto", Cif = "11111a", Direccion = "Calle1" },
            //new Proveedor { Id = 2, Nombre = "Maria", Cif = "22222b", Direccion = "Calle2" }
            ingrProvVM.IdProveedor = int.Parse(IdProveedor);
            //new Ingrediente { Id = 1, Nombre = "Lechuga", PrecioUnitario = 2, Stock = 8, AlergSandws = new List<AlergSandw> { new AlergSandw { Id = 1, IngredienteId = 1, Alergeno = alergenos[0] } } },
            //new Ingrediente { Id = 2, Nombre = "Tomate", PrecioUnitario = 3, Stock = 9, AlergSandws = new List<AlergSandw> { new AlergSandw { Id = 2, IngredienteId = 2, Alergeno = alergenos[1] } } }

            ingrProvVM.ingredienteNombre = ingrNombre;
            ingrProvVM.ingredienteStock = 8;

            var result = controller.Create(ingrProvVM); // Init Controller parameter

            Ingrediente expectedIngr = UtilitiesForIngredientes.GetIngredientes(0, 1).First();
            //IngrProv expectedIngrProv = UtilitiesForIngredientes.GetIngrProv(0, 1).First();

            IngrPedProvViewModel detallePedidoProvCreateViewModel = new IngrPedProvViewModel()
            {
                Id = expectedIngr.Id,
                IngrProvId = expectedIngr.IngrProv.First().Id,
                IngrProv = expectedIngr.IngrProv.First(),
                PedidoProvId = expectedIngr.IngrProv.First().IngrPedProvs.First().PedidoProv.Id,
                PedidoProv = expectedIngr.IngrProv.First().IngrPedProvs.First().PedidoProv,
                Cantidad = expectedIngr.IngrProv.First().IngrPedProvs.First().Cantidad,
                //NombreIngrediente = expectedIngr.Nombre,
                //PrecioUnitario = expectedIngr.PrecioUnitario,
                //Stock = expectedIngr.Stock,
                //Alergenos = new List<string>(),
            };

            IList<IngrPedProvViewModel> expectedDetallePedido = new
                IngrPedProvViewModel[1] { detallePedidoProvCreateViewModel };

            PedidoProvCreateViewModel expectedPedidoProvVM = new PedidoProvCreateViewModel(
                proveedor.IngrProv.First().IngrPedProvs.First().Id,
                proveedor.Cif,
                proveedor.Nombre,
                proveedor.Direccion,
                proveedor.IngrProv.First().IngrPedProvs.First().Cantidad,
                proveedor.Id,
                proveedor.IngrProv.First().IngrPedProvs.First(),
                proveedor.Direccion,
                proveedor.IngrProv.First().IngrPedProvs.First().PedidoProv.MetodoDePago.ToString(),
                proveedor.IngrProv.First().IngrPedProvs.First().PedidoProv.DireccionEnvio,
                proveedor.IngrProv.First().IngrPedProvs.First().PedidoProv.FechaPedido,
                proveedor.IngrProv.First().IngrPedProvs.First().PedidoProv.PrecioTotal
             );


            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
            PedidoProvCreateViewModel currentPedido = viewResult.Model as PedidoProvCreateViewModel;

            Assert.Equal(expectedPedidoProvVM, currentPedido);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutIngrediente()
        {
            // Arrange
            var controller = new PedidoProvsController(context);
            //simulate user's connection
            controller.ControllerContext.HttpContext = httpContext;

            SelectedIngrProvForPurchaseViewModel ingrProvVM = new();
            PedidoProvCreateViewModel expectedPedidoProvVM = new PedidoProvCreateViewModel(
                proveedor.IngrProv.First().IngrPedProvs.First().Id,
                proveedor.Cif,
                proveedor.Nombre,
                proveedor.Direccion,
                proveedor.IngrProv.First().IngrPedProvs.First().Cantidad,
                proveedor.Id,
                proveedor.IngrProv.First().IngrPedProvs.First(),
                proveedor.Direccion,
                proveedor.IngrProv.First().IngrPedProvs.First().PedidoProv.MetodoDePago.ToString(),
                proveedor.IngrProv.First().IngrPedProvs.First().PedidoProv.DireccionEnvio,
                proveedor.IngrProv.First().IngrPedProvs.First().PedidoProv.FechaPedido,
                proveedor.IngrProv.First().IngrPedProvs.First().PedidoProv.PrecioTotal
            );



            //Act
            var result = controller.Create(ingrProvVM);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
            PedidoProvCreateViewModel currentPedido = viewResult.Model as PedidoProvCreateViewModel;

            Assert.Equal(expectedPedidoProvVM, currentPedido);
        }

        public static IEnumerable<object[]> TestCasesForPedidoProvCreatePost_WithoutErrors()
        {
            Proveedor proveedor = UtilitiesForIngredientes.GetProveedores(0, 1) as Proveedor;
            IList<MetodoDePago> paymentMethods = UtilitiesForPedidoProvs.GetPaymentMethods(0, 1);
            IList<Ingrediente> ingr = UtilitiesForIngredientes.GetIngredientes(0, 1);
            IList<PedidoProv> pedidos = UtilitiesForPedidoProvs.GetPedidoProvs(0, 2, paymentMethods, ingr, gerente);

            IList<int> ExpectedQuantities = UtilitiesForIngredientes.GetIngredientes(0, 1)
            .Select(m => m.Stock).ToList();


            //Purchase with Tarjeta
            PedidoProv expectedPedido1 = pedidos[0];
            expectedPedido1.Id = 1;
            expectedPedido1.MetodoDePago.Id = 1;
            PedidoProvCreateViewModel stockVM1 = new PedidoProvCreateViewModel(expectedPedido1);


            int expectedCantidadForPedido1 = ExpectedQuantities[0] - stockVM1.ingredientesPedProv[0].Cantidad;

            //Purchase with Efectivo
            PedidoProv expectedPedido2 = pedidos[1];
            expectedPedido2.Id = 2;
            expectedPedido2.MetodoDePago.Id = 2;
            PedidoProvCreateViewModel stockVM2 = new PedidoProvCreateViewModel(expectedPedido2);
            int expectedCantidadForPedido2 = ExpectedQuantities[1] - stockVM2.ingredientesPedProv[0].Cantidad;


            var allTests = new List<object[]> {
                new object[] { stockVM1,expectedPedido1,expectedCantidadForPedido1},
                new object[] { stockVM2,expectedPedido2,expectedCantidadForPedido2}
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForPedidoProvCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(PedidoProvCreateViewModel pedidoprov,
            PedidoProv expectedPedido, int expectedCantidadForPedido)
        {
            var controller = new PedidoProvsController(context);
            controller.ControllerContext.HttpContext = httpContext;

            //Act
            var result = controller.Create(pedidoprov);
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

            Assert.Equal("Details", viewResult.ActionName);
        }

        //public static IEnumerable<object[]> TestCasesForPedidoProvCreatePost_WithErrors()
        //{
        //    Ingrediente ingrStock = UtilitiesForIngredientes.GetIngredientes(0, 1).First();
        //    //Proveedor proveedor= UtilitiesforApplicationUser.GetUsers(1, 1).First() as Proveedor;

        //    var cantidadErronea = 9999;

        //    IngrPedProvViewModel detalleView = new IngrPedProvViewModel()
        //    {

        //        Id = detalleView.Id,
        //        N
        //    };
        //    IList<DetallePedidoCreateViewModel> detallePedidoCreateViewModels = new DetallePedidoCreateViewModel[1] { detalleView };
        //    PedidoCreateViewModel pedido1 = new PedidoCreateViewModel(mecanico, detallePedidoCreateViewModels);

        //    DetallePedidoCreateViewModel detalleView2 = new DetallePedidoCreateViewModel()
        //    {
        //        Cantidad = cantidadErronea,
        //        PiezaId = piezaStock.Id,
        //        PiezaMarca = piezaStock.Marca.Name,
        //        PiezaModelo = piezaStock.Modelo,
        //        PiezaNombre = piezaStock.Nombre,
        //        PiezaTipo = piezaStock.Tipo_pieza,
        //        Precio = piezaStock.PrecioCompra,
        //    };
        //    IList<DetallePedidoCreateViewModel> detallePedidoCreateViewModels2 = new DetallePedidoCreateViewModel[1] { detalleView2 };
        //    PedidoCreateViewModel expectedPedido = new PedidoCreateViewModel(mecanico, detallePedidoCreateViewModels2);
        //    string expectedErrorMessage1 = $"Has seleccionado una cantidad de stock mayor a la que hay en {piezaStock.Nombre}";


        //    var allTests = new List<object[]> {
        //        new object[] { pedido1,expectedPedido,expectedErrorMessage1}
        //    };
        //    return allTests;
        //}
    }
}
