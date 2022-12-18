using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.PedidoSandwichPersonalizadoViewModels;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using Sandwich2Go.Models.IngredienteViewModels;

namespace Sandwich2Go.UT.PedidoPersonalizadoController_test
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
        public void CreatePedido_IngredientesNotSelected()
        {
            using (context)
            {
                //Arrange
                var controller = new PedidosController(context);

                controller.ControllerContext.HttpContext = pedidosHttpContext;
                Cliente cliente = Utilities.GetUsers(1, 1).First() as Cliente;
                var expectedViewModel = new PedidoCreateSandwichPersonalizadoViewModel
                {
                    Name = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    IdCliente = "2",
                    necesitaCambio = false,
                    ingPedidos = UtilitiesForPedido.GetIngredientes(0, 0).OrderBy(s => s.Nombre).Select(s => new IngredientePedidoViewModel(s)).ToList(),
                };

                SelectedIngredientesForPurchaseViewModel selected = new SelectedIngredientesForPurchaseViewModel { IdsToAdd = null, ingredienteAlergenoSelected = null, ingredienteNombre = "0" };

                //Act

                var result = controller.CreateSandwichPersonalizado(selected);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);

                PedidoCreateSandwichPersonalizadoViewModel model = viewResult.Model as PedidoCreateSandwichPersonalizadoViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                //Comprobamos igualdad entre ViewModels
                Assert.Equal(expectedViewModel, model);
                Assert.Equal("Debes elegir al menos un ingrediente para crear un pedido.", error.ErrorMessage);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void CreatePedido_IngredientesSelected()
        {
            using (context)
            {
                //Arrange
                var controller = new PedidosController(context);

                controller.ControllerContext.HttpContext = pedidosHttpContext;
                Cliente cliente = Utilities.GetUsers(1, 1).First() as Cliente;
                var expectedViewModel = new PedidoCreateSandwichPersonalizadoViewModel
                {
                    Name = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    IdCliente = "2",
                    necesitaCambio = false,
                    ingPedidos= UtilitiesForPedido.GetIngredientes(0, 2).OrderBy(s => s.Nombre).Select(s => new IngredientePedidoViewModel(s)).ToList(),
                };

                SelectedIngredientesForPurchaseViewModel selected = new SelectedIngredientesForPurchaseViewModel { IdsToAdd = new string[] { "1", "2" }, ingredienteAlergenoSelected = null, ingredienteNombre = "0" };

                //Act

                var result = controller.CreateSandwichPersonalizado(selected);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);

                PedidoCreateSandwichPersonalizadoViewModel model = viewResult.Model as PedidoCreateSandwichPersonalizadoViewModel;
                //Comprobamos igualdad entre ViewModels
                Assert.Equal(expectedViewModel, model);
            }
        }

        public static IEnumerable<object[]> TestCasesForCompraCreatePost_WithErrors()
        {
            UtilitiesForPedido.CrearDatos();
            Ingrediente ingrediente1 = UtilitiesForPedido.GetIngredientes(0, 1).First();
            Cliente cliente = Utilities.GetUsers(1, 1).First() as Cliente;

            //Primer error: Sándwich con cantidad inválida
            IngredientePedidoViewModel sp1 = new IngredientePedidoViewModel(ingrediente1);
            sp1.cantidad = 0;
            //Valores input

            IList<IngredientePedidoViewModel> ingredientesPedidos1 = new IngredientePedidoViewModel[1]
            {
                sp1
            };

            PedidoCreateSandwichPersonalizadoViewModel pedidoInput1 = new PedidoCreateSandwichPersonalizadoViewModel();

            pedidoInput1.ingPedidos.Add(ingredientesPedidos1[0]);
            pedidoInput1.IdCliente = cliente.Id;
            pedidoInput1.Name = cliente.Nombre;
            pedidoInput1.Apellido = cliente.Apellido;

            //Valores esperados
            PedidoCreateSandwichPersonalizadoViewModel pedidoEsperado1 = new PedidoCreateSandwichPersonalizadoViewModel();

            pedidoEsperado1.ingPedidos.Add(ingredientesPedidos1[0]);
            pedidoEsperado1.IdCliente = cliente.Id;
            pedidoEsperado1.Name = cliente.Nombre;
            pedidoEsperado1.Apellido = cliente.Apellido;

            string errorEsperado1 = "El restaurante no puede preparar en estos momentos el sándwich Cubano, por favor, selecciona una cantidad distinta o no lo incluyas en el pedido.";

            //Segundo error: Sandwich sin ingredientes suficientes

            Ingrediente ingrediente2 = UtilitiesForPedido.GetIngredientes(0, 1).First();
            IngredientePedidoViewModel sp2 = new IngredientePedidoViewModel(ingrediente2);
            sp2.cantidad = 200;//No hay cantidad suficiente de ingredientes para preparar los sándwiches
            //Valores input
            IList<IngredientePedidoViewModel> ingredientesPedidos2 = new IngredientePedidoViewModel[1]
            {
                sp2
            };

            PedidoCreateSandwichPersonalizadoViewModel pedidoInput2 = new PedidoCreateSandwichPersonalizadoViewModel();

            pedidoInput2.ingPedidos.Add(ingredientesPedidos2[0]);
            pedidoInput2.IdCliente = cliente.Id;
            pedidoInput2.Name = cliente.Nombre;
            pedidoInput2.Apellido = cliente.Apellido;

            //Valores esperados
            PedidoCreateSandwichPersonalizadoViewModel pedidoEsperado2 = new PedidoCreateSandwichPersonalizadoViewModel();

            pedidoEsperado2.ingPedidos.Add(ingredientesPedidos2[0]);
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

       

        }
    }
//}
