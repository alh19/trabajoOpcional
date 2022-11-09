using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.IngredienteViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Sandwich2Go.UT.IngredientesController_test
{
    public class SelectIngredientes_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;

        public SelectIngredientes_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the InMemory database with test data.
            UtilitiesForIngredientes.InitializeDbIngredientesForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            purchaseContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            purchaseContext.User = identity;

        }
        public static IEnumerable<object[]> TestCasesForSelectIngredientesForPurchase_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForIngredientes.GetIngredientes(0,3), UtilitiesForIngredientes.GetAlergenos(0,4), null, null },
                new object[] { UtilitiesForIngredientes.GetIngredientes(0,1), UtilitiesForIngredientes.GetAlergenos(0,4), "Lechuga", null},
                new object[] { UtilitiesForIngredientes.GetIngredientes(2,1), UtilitiesForIngredientes.GetAlergenos(0,4), null, "Gluten"},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectIngredientesForPurchase_get))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task SelectIngredientesForPurchase_Get(List<Ingrediente> expectedIngredientes, List<Alergeno> expectedAlergenos, string filterNombre, string filterAlergeno)
        {
            using (context)
            {

                // Arrange
                var controller = new IngredientesController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                var expectedAlergenosNames = expectedAlergenos.Select(g => new { nameofAlergeno = g.Name });

                // Act
                var result = controller.SelectIngredientesForPurchase(filterNombre, filterAlergeno);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectIngredientesForPurchaseViewModel model = viewResult.Model as SelectIngredientesForPurchaseViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                Assert.Equal(expectedIngredientes, model.Ingredientes);
                //check that both collections (expected and result) have the same names of Genre
                var modelAlergenos = model.Alergenos.Select(g => new { nameofAlergeno = g.Text });
                Assert.True(expectedAlergenosNames.SequenceEqual(modelAlergenos));
            }
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectIngredientesForPurchase_Post_IngredientesNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new IngredientesController(context);
                controller.ControllerContext.HttpContext = purchaseContext;
                //we create an array that is a list names of genres
                var expectedAlergenos = UtilitiesForIngredientes.GetAlergenos(0, 4).Select(g => new { nameofAlergeno = g.Name });
                var expectedIngredientes = UtilitiesForIngredientes.GetIngredientes(0, 3);

                SelectedIngredientesForPurchaseViewModel selected = new SelectedIngredientesForPurchaseViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectIngredientesForPurchase(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectIngredientesForPurchaseViewModel model = viewResult.Model as SelectIngredientesForPurchaseViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedIngredientes, model.Ingredientes);

                //check that both collections (expected and result) have the same names of Genre
                var modelAlergenos = model.Alergenos.Select(g => new { nameofAlergeno = g.Text });
                Assert.True(expectedAlergenos.SequenceEqual(modelAlergenos));

            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectIngredientesForPurchase_Post_IngredientesSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new IngredientesController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                String[] ids = new string[1] { "1" };
                SelectedIngredientesForPurchaseViewModel ingredientes = new SelectedIngredientesForPurchaseViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectIngredientesForPurchase(ingredientes);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentIngredientes = viewResult.RouteValues.Values.First();
                Assert.Equal(ingredientes.IdsToAdd, currentIngredientes);

            }
        }
    }
}
