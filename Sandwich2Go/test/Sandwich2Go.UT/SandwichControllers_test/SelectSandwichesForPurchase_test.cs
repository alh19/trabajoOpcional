using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.SandwichViewModels;
using Sandwich2Go.UT.SandwichControllers_test;
using Xunit;

namespace Sandwich2Go.UT.SandwichControllers_test
{
    

    public class SelectSandwichesForPurchase_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext sandwichesHttpContext;

        public SelectSandwichesForPurchase_test()
        {
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            UtilitiesForSandwiches.InitializeDbSandwichesForTests(context);
            //Conexión de usuario
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            sandwichesHttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            sandwichesHttpContext.User = identity;

        }

        public static IEnumerable<object[]> TestCasesForSelect_AlergenoSelected()
        {
            UtilitiesForSandwiches.CrearDatos();
            var allTest = new List<object[]>
            {
                new object[]{new SelectSandwichesViewModel
                {
                    Sandwiches = UtilitiesForSandwiches.GetSandwiches(0,3).OrderBy(s=>s.SandwichName).ToList().Select(s=>new SandwichForPurchaseViewModel(s)),
                    sandwichPrecio =0,
                    sandwichAlergenoSelected = null,
                    Alergenos =new SelectList(UtilitiesForSandwiches.GetAlergenos(0, 2).Select(a => a.Name)),
                },0,null},//No introducimos precio ni alérgeno, por lo que esperamos todos los sándwiches

                new object[]{new SelectSandwichesViewModel
                {
                    Sandwiches = UtilitiesForSandwiches.GetSandwiches(0,0).OrderBy(s=>s.SandwichName).ToList().Select(s=>new SandwichForPurchaseViewModel(s)),
                    sandwichPrecio =0,
                    sandwichAlergenoSelected = "Leche",
                    Alergenos =new SelectList(UtilitiesForSandwiches.GetAlergenos(0, 2).Select(a => a.Name))
                },0,"Leche"},//No introducimos precio. Alérgeno Leche. No esperamos ningún sándwich.

                new object[]{new SelectSandwichesViewModel
                {
                    Sandwiches = UtilitiesForSandwiches.GetSandwiches(1,2).OrderBy(s=>s.SandwichName).ToList().Select(s=>new SandwichForPurchaseViewModel(s)),
                    sandwichPrecio =4,
                    sandwichAlergenoSelected = null,
                    Alergenos =new SelectList(UtilitiesForSandwiches.GetAlergenos(0, 2).Select(a => a.Name))
                },4,null},//Introducimos precio y no alérgeno. Esperamos 2 sándwiches.

                new object[]{new SelectSandwichesViewModel
                {
                    Sandwiches = UtilitiesForSandwiches.GetSandwiches(1,1).OrderBy(s=>s.SandwichName).ToList().Select(s=>new SandwichForPurchaseViewModel(s)),
                    sandwichPrecio =3,
                    sandwichAlergenoSelected = "Huevo",
                    Alergenos =new SelectList(UtilitiesForSandwiches.GetAlergenos(0, 2).Select(a => a.Name))
                },3,"Huevo"}//Introducimos precio y alérgeno. No esperamos ningún sándwich.
            };

            UtilitiesForSandwiches.BorrarDatos();

            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelect_AlergenoSelected))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Select_AlergenoSelectedAndPrecio(SelectSandwichesViewModel expectedModel, double precio, string alergeno)
        {
            using (context)
            {
                //Arrange
                var controller = new SandwichesController(context);
                //Act
                var result = controller.SelectSandwichForPurchase(precio, alergeno);
                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                SelectSandwichesViewModel viewModel = viewResult.Model as SelectSandwichesViewModel;
                //Comprobamos igualdad entre ViewModels
                Assert.Equal(expectedModel, viewModel);

            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectSandwichesForPurchase_Post_SandwichesNotSelected()
        {
            using (context)
            {
                //Arrange
                var controller = new SandwichesController(context);

                var expectedViewModel = new SelectSandwichesViewModel()
                {
                    Sandwiches = UtilitiesForSandwiches.GetSandwiches(0, 3).OrderBy(s => s.SandwichName).ToList().Select(s => new SandwichForPurchaseViewModel(s)),
                    sandwichPrecio = 0,
                    sandwichAlergenoSelected = null,
                    Alergenos = new SelectList(UtilitiesForSandwiches.GetAlergenos(0, 2).Select(a => a.Name))
                };

                SelectedSandwichesForPurchaseViewModel selected = new SelectedSandwichesForPurchaseViewModel { IdsToAdd = null, sandwichAlergenoSelected = null, sandwichPrecio = "0" };

                //Act

                var result = controller.SelectSandwichForPurchase(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);

                SelectSandwichesViewModel model = viewResult.Model as SelectSandwichesViewModel;
                //Comprobamos igualdad entre ViewModels
                Assert.Equal(expectedViewModel, model);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectSandwichesForPurchase_Post_SandwichesSelected()
        {
            using (context)
            {
                //Arrange
                var controller = new SandwichesController(context);
                controller.ControllerContext.HttpContext = sandwichesHttpContext;

                String[] ids = new string[2] { "1", "2" };
                SelectedSandwichesForPurchaseViewModel sandwiches = new SelectedSandwichesForPurchaseViewModel { IdsToAdd = ids, sandwichAlergenoSelected = null, sandwichPrecio = "0" };

                //Act
                var result = controller.SelectSandwichForPurchase(sandwiches);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentSandwiches = viewResult.RouteValues.Values.First();
                Assert.Equal(sandwiches.IdsToAdd, currentSandwiches);

            }
        }

    }
}
