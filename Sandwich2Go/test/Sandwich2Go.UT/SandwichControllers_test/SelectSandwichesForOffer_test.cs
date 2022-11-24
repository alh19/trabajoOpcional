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
using Xunit;


namespace Sandwich2Go.UT.SandwichControllers_test
{
    public class SelectSandwichesForOffer_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext ofertaContext;

        public SelectSandwichesForOffer_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the InMemory database with test data.
            UtilitiesForSandwichesForOffer.InitializeDbSandwichesForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            ofertaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            ofertaContext.User = identity;

        }
        public static IEnumerable<object[]> TestCasesForSelectSandwichesForOffer_get()
        {

            var sandwichesForOfferVMTC1 = new SelectSandwichesForOfferViewModel()
            {
                Sandwiches = UtilitiesForSandwichesForOffer.GetSandwiches(0, 2)
                        .OrderBy(i => i.Id)
                        .Select(h => new SandwichForOfferViewModel(h)).ToList()
            };

            var sandwichesForOfferVMTC2 = new SelectSandwichesForOfferViewModel()
            {
                Sandwiches = UtilitiesForSandwichesForOffer.GetSandwiches(0, 1)
                        .OrderBy(i => i.Id)
                        .Select(h => new SandwichForOfferViewModel(h)).ToList()
            };

            var sandwichesForOfferVMTC3 = new SelectSandwichesForOfferViewModel()
            {
                Sandwiches = UtilitiesForSandwichesForOffer.GetSandwiches(1, 1)
                        .OrderBy(i => i.Id)
                        .Select(h => new SandwichForOfferViewModel(h)).ToList()
            };
            var allTests = new List<object[]>
                {
                    new object[] { sandwichesForOfferVMTC1, null, 0.00},
                    new object[] { sandwichesForOfferVMTC2, "Cubano", 0.00},
                    new object[] { sandwichesForOfferVMTC3, null, 3.50},
                  };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectSandwichesForOffer_get))]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectSandwichesForOffer_Get(SelectSandwichesForOfferViewModel expectedSandwiches, string sandwichNombre, double sandwichPrecio)
        {

            var controller = new SandwichesController(context);

            var result = controller.SelectSandwichesForOffer(sandwichNombre, sandwichPrecio);


            var viewResult = Assert.IsType<ViewResult>(result.Result);
            SelectSandwichesForOfferViewModel model = viewResult.Model as SelectSandwichesForOfferViewModel;

            Assert.Equal(expectedSandwiches, model);
        }



        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectSandwichesForOffer_Post_SandwichesNotSelected()
        {



            var controller = new SandwichesController(context);
            var expectedSelectSandwichesForOfferViewModel = new SelectSandwichesForOfferViewModel()
            {
                Sandwiches = UtilitiesForSandwichesForOffer.GetSandwiches(0, 2)
                        .OrderBy(i => i.Id)
                        .Select(h => new SandwichForOfferViewModel(h)).ToList()
            };

            SelectedSandwichesForOfferViewModel selected = new SelectedSandwichesForOfferViewModel { IdsToAdd = null };


            var result = controller.SelectSandwichesForOffer(selected);


            var viewResult = Assert.IsType<ViewResult>(result.Result);
            SelectSandwichesForOfferViewModel model = viewResult.Model as SelectSandwichesForOfferViewModel;


            Assert.Equal(expectedSelectSandwichesForOfferViewModel, model);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectSandwichesForOffer_Post_SandwichesSelected()
        {


            // Arrange
            var controller = new SandwichesController(context);
            controller.ControllerContext.HttpContext = ofertaContext;

            String[] ids = new string[1] { "1" };
            SelectedSandwichesForOfferViewModel sandwiches = new SelectedSandwichesForOfferViewModel { IdsToAdd = ids };

            // Act
            var result = controller.SelectSandwichesForOffer(sandwiches);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            var currentSandwiches = viewResult.RouteValues.Values.First();
            Assert.Equal(sandwiches.IdsToAdd, currentSandwiches);


        }

    }
}
