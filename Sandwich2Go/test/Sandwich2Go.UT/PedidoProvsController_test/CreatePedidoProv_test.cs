using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.IngredienteViewModels;
using Sandwich2Go.Models.PedidoProvViewModels;
using Sandwich2Go.Models.PedidoViewModels;
using Sandwich2Go.Models.SandwichViewModels;
using Sandwich2Go.UT.SandwichControllers_test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sandwich2Go.UT.PedidoProvsController_test
{
    public class CreatePedidoProv_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext pedidosHttpContext;

        public CreatePedidoProv_test()
        {
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            UtilitiesForPedidoProv.InitializeDbPedidoProvsForTests(context);

            //Conexión de usuario
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            pedidosHttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            pedidosHttpContext.User = identity;
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void CreatePedidoProv_IngredientesNotSelected()
        {
            using (context)
            {
                //Arrange
                var controller = new PedidoProvsController(context);

                controller.ControllerContext.HttpContext = pedidosHttpContext;
                Gerente gerente = Utilities.GetUsers(0, 0).First() as Gerente;
                var expectedViewModel = new PedidoProvCreateViewModel
                {
                    Id = 1,
                    Cif = UtilitiesForPedidoProv.GetProveedores(0,0).First().Cif,
                    NombreProveedor = UtilitiesForPedidoProv.GetProveedores(0, 0).First().Nombre,
                    Direccion = UtilitiesForPedidoProv.GetProveedores(0, 0).First().Direccion,
                    IdProveedor = UtilitiesForPedidoProv.GetProveedores(0, 0).First().Id,
                    ingredientesPedProv = UtilitiesForPedidoProv.GetIngredientes(0, 0).OrderBy(s => s.Nombre).Select(s => new PedidoProvCreateViewModel.IngrPedProvViewModel(s)).ToList(),
                };

                SelectedIngrProvForPurchaseViewModel selected = new SelectedIngrProvForPurchaseViewModel { IdsToAdd = null, ingredienteStock = 0, ingredienteNombre = "" };

                //Act
                var result = controller.Create(selected);

                //Assert
                ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);

                PedidoSandwichCreateViewModel model = viewResult.Model as PedidoSandwichCreateViewModel;
                var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
                //Comprobamos igualdad entre ViewModels
                Assert.Equal(expectedViewModel.ToString(), (IEnumerable<char>)model);
                Assert.Equal("Debes seleccionar al menos un ingrediente", error.ErrorMessage);
            }
        }
    }
}
