using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        Microsoft.AspNetCore.Http.DefaultHttpContext ingredienteContext;

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
            ingredienteContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            ingredienteContext.User = identity;

        }
        public static IEnumerable<object[]> TestCasesForSelectIngredientesForPurchase_get()
        {
            var alergenosNames = new SelectList(UtilitiesForIngredientes.GetAlergenos(0, 4).Select(a => a.Name));
            var ingredientesForPurchaseVMTC1 = new SelectIngredientesForPurchaseViewModel()
            {
                Alergenos = alergenosNames,
                Ingredientes = UtilitiesForIngredientes.GetIngredientes(0, 2)
                        .OrderBy(i => i.Id)
                        .Select(h => new IngredienteForPurchaseViewModel(h)).ToList()
            };
           
            var ingredientesForPurchaseVMTC2 = new SelectIngredientesForPurchaseViewModel()
            {
                Alergenos = alergenosNames,
                Ingredientes = UtilitiesForIngredientes.GetIngredientes(0, 2)
                        .OrderBy(i => i.Id)
                        .Select(h => new IngredienteForPurchaseViewModel(h)).ToList()
            };

            var ingredientesForPurchaseVMTC3 = new SelectIngredientesForPurchaseViewModel()
            {
                Alergenos = alergenosNames,
                Ingredientes = UtilitiesForIngredientes.GetIngredientes(0, 2)
                        .OrderBy(i => i.Id)
                        .Select(h => new IngredienteForPurchaseViewModel(h)).ToList()
            };
            var allTests = new List<object[]>
                {
                    new object[] { ingredientesForPurchaseVMTC1, null, null},
                    new object[] { ingredientesForPurchaseVMTC2, "Gluten", null},
                    new object[]{ ingredientesForPurchaseVMTC3, null,"Lechuga" },
                  };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectIngredientesForPurchase_get))]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectIngredientesForPurchase_Get(SelectIngredientesForPurchaseViewModel expectedIngredientes, string ingredienteAlergenoSelected, string ingredienteNombre)
        {

            var controller = new IngredientesController(context);




            var result = controller.SelectIngredientesForPurchase(ingredienteAlergenoSelected, ingredienteNombre);


            var viewResult = Assert.IsType<ViewResult>(result.Result);
            SelectIngredientesForPurchaseViewModel model = viewResult.Model as SelectIngredientesForPurchaseViewModel;

            Assert.Equal(expectedIngredientes, model);
        }
    


    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public void SelectIngredientesForPurchase_Post_IngredientesNotSelected()
    {
        


            var controller = new IngredientesController(context);
            var alergenosNames = new SelectList(UtilitiesForIngredientes.GetAlergenos(0, 4).Select(a => a.Name));
            var expectedSelectIngredientesForPurchaseViewModel = new SelectIngredientesForPurchaseViewModel()
            {
                Alergenos = alergenosNames,
                Ingredientes = UtilitiesForIngredientes.GetIngredientes(0, 2)
                        .OrderBy(i => i.Id)
                        .Select(h => new IngredienteForPurchaseViewModel(h)).ToList()
            };

            SelectedIngredientesForPurchaseViewModel selected = new SelectedIngredientesForPurchaseViewModel { IdsToAdd = null };


            var result = controller.SelectIngredientesForPurchase(selected);


            var viewResult = Assert.IsType<ViewResult>(result.Result);
            SelectIngredientesForPurchaseViewModel model = viewResult.Model as SelectIngredientesForPurchaseViewModel;


            Assert.Equal(expectedSelectIngredientesForPurchaseViewModel, model);
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public void SelectIngredientesForPurchase_Post_IngredientesSelected()
    {
        

            // Arrange
            var controller = new IngredientesController(context);
            controller.ControllerContext.HttpContext = ingredienteContext;

            String[] ids = new string[1] { "1" };
            SelectedIngredientesForPurchaseViewModel ingredientes = new SelectedIngredientesForPurchaseViewModel { IdsToAdd = ids };

            // Act
            var result = controller.SelectIngredientesForPurchase(ingredientes);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            var currentIngredientes = viewResult.RouteValues.Values.First();
            Assert.Equal(ingredientes.IdsToAdd, currentIngredientes);

        
    }

}
}
