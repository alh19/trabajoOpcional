using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.SandwichViewModels;
using Sandwich2Go.Models.PedidoViewModels;
using Sandwich2Go.UT.ProveedoresController_test;
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

            UtilitiesForPedido.InitializeDbSandwichesForTests(context);
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
                    PrecioFinal = 0,
                    sandwichesPedidos = UtilitiesForPedido.GetSandwiches(0, 0).OrderBy(s => s.SandwichName).Select(s => new SandwichPedidoViewModel(s)).ToList(),
                    PrecioTotal = 0
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
                Assert.Equal("Debes elegir al menos un sándwich para crear un pedido.",error.ErrorMessage);
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
                    PrecioFinal = 8.5,
                    sandwichesPedidos = UtilitiesForPedido.GetSandwiches(0, 2).OrderBy(s => s.SandwichName).Select(s => new SandwichPedidoViewModel(s)).ToList(),
                    PrecioTotal = 8.5
                };

                SelectedSandwichesForPurchaseViewModel selected = new SelectedSandwichesForPurchaseViewModel { IdsToAdd = new string[] {"1","2"}, sandwichAlergenoSelected = null, sandwichPrecio = "0" };

                //Act

                var result = controller.Create(selected);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);

                PedidoSandwichCreateViewModel model = viewResult.Model as PedidoSandwichCreateViewModel;
                //Comprobamos igualdad entre ViewModels
                Assert.Equal(expectedViewModel, model);
            }
        }
    }
}
