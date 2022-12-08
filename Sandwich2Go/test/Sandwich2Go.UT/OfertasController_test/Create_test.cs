using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.OfertaViewModels;
using Sandwich2Go.Models.SandwichViewModels;
using Sandwich2Go.UT.SandwichControllers_test;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sandwich2Go.UT.OfertasController_test
{
    public class Create_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext httpContext;
        IList<Sandwich> sandwiches;
        Gerente gerente;

        public Create_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();
            UtilitiesForSandwichesForOffer.InitializeDbSandwichesForTests(context);
            sandwiches = context.Sandwich.ToList();
            //Utilities.InitializeDbCustomersForTests(context);
            gerente = context.Users.First() as Gerente;

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = identity
            };

        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithSelectedSandwiches()
        {
            // Arrange

            var controller = new OfertasController(context);
            //simulate user's connection
            controller.ControllerContext.HttpContext = httpContext;


            SelectedSandwichesForOfferViewModel sandwichesVM = new() { IdsToAdd = new string[1] { "1" } };

            OfertaSandwichViewModel ofertaSandwichViewModel = new OfertaSandwichViewModel(sandwiches.First());
            OfertaCreateViewModel expectedOfertaVM = new OfertaCreateViewModel(gerente, new OfertaSandwichViewModel[1] { ofertaSandwichViewModel });


            // Act
            var result = controller.Create(sandwichesVM);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
            OfertaCreateViewModel currentOferta = viewResult.Model as OfertaCreateViewModel;

            Assert.Equal(expectedOfertaVM, currentOferta);


        }
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Get_WithoutSandwich()
        {
            // Arrange
            var controller = new OfertasController(context);
            //simulate user's connection
            controller.ControllerContext.HttpContext = httpContext;

            SelectedSandwichesForOfferViewModel selectedSandwichesForOffer = new SelectedSandwichesForOfferViewModel();
            OfertaCreateViewModel expectedOferta = new OfertaCreateViewModel(gerente, new List<OfertaSandwichViewModel>());


            // Act
            var result = controller.Create(selectedSandwichesForOffer);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
            OfertaCreateViewModel currentOferta = viewResult.Model as OfertaCreateViewModel;
            var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
            Assert.Equal(expectedOferta, currentOferta);
            Assert.Equal("Debes elegir al menos un sándwich para crear una oferta.", error.ErrorMessage);

        }
        
        public static IEnumerable<object[]> TestCasesForOffersCreatePost_WithErrors()
        {
            Sandwich sandwichToOffer = UtilitiesForSandwichesForOffer.GetSandwiches(0, 1).First();
            Gerente gerente = Utilities.GetUsers(0, 1).First() as Gerente;

            //Input values
            OfertaSandwichViewModel ofertaSandwichViewModel = new OfertaSandwichViewModel(sandwichToOffer);
            ofertaSandwichViewModel.Porcentaje = 200;
            IList<OfertaSandwichViewModel> ofertaSandwichViewModel1 = new OfertaSandwichViewModel[1] { ofertaSandwichViewModel };
            OfertaCreateViewModel oferta1 = new(gerente, ofertaSandwichViewModel1);


            //Expected values
            OfertaSandwichViewModel expectedOfertaSandwichViewModel = new OfertaSandwichViewModel(sandwichToOffer);
            expectedOfertaSandwichViewModel.Porcentaje = 200;
            IList<OfertaSandwichViewModel> expectedOfertaSandwichViewModel1 = new OfertaSandwichViewModel[1] { expectedOfertaSandwichViewModel };
            OfertaCreateViewModel expectedOfertaVM1 = new(gerente, expectedOfertaSandwichViewModel1);

            string expetedErrorMessage1 = "Introduce un porcentaje válido para el sándwich Cubano";

            var allTests = new List<object[]>
            {                  
                new object[] { oferta1, expectedOfertaVM1, expetedErrorMessage1 }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForOffersCreatePost_WithErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithErrors(OfertaCreateViewModel oferta, OfertaCreateViewModel expectedOfertaVM, string errorMessage)
        {
            // Arrange
            var controller = new OfertasController(context);
            //simulate user's connection
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = controller.CreatePost(oferta);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            OfertaCreateViewModel currentOferta = viewResult.Model as OfertaCreateViewModel;
            Assert.Equal(expectedOfertaVM, currentOferta);

            var error = viewResult.ViewData.ModelState.Values.First().Errors.First();
            Assert.Equal(errorMessage, error.ErrorMessage);

        }

        public static IEnumerable<object[]> TestCasesForOffersCreatePost_WithoutErrors()
        {
            Gerente gerente = Utilities.GetUsers(0, 1).First() as Gerente;
            IList<Sandwich> sandwiches = UtilitiesForSandwichesForOffer.GetSandwiches(0, 3);
            IList<Oferta> ofertaList = UtilitiesForOfertas.GetOfertas(0, 2, sandwiches, gerente);

            Oferta expectedOferta1 = ofertaList[0];
            OfertaCreateViewModel ofertaCVM1 = new OfertaCreateViewModel(expectedOferta1);

            Oferta expectedOferta2 = ofertaList[1];
            expectedOferta2.Id = 1;
            OfertaCreateViewModel ofertaCVM2 = new OfertaCreateViewModel(expectedOferta2);

            var allTests = new List<object[]>
            {                  
                new object[] { ofertaCVM1, expectedOferta1},
                new object[] { ofertaCVM2, expectedOferta2}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForOffersCreatePost_WithoutErrors))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Create_Post_WithoutErrors(OfertaCreateViewModel oferta, Oferta expectedOferta)
        {
            // Arrange
            var controller = new OfertasController(context);

            //simulate user's connection
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = controller.CreatePost(oferta);

            //Assert
            //we should check it is redirected to details
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Details", viewResult.ActionName);
            //it is the id of the purchase stored in the DB and passed to details
            Assert.Equal(expectedOferta.Id, viewResult.RouteValues.First().Value);

            //we should check the purchase has been created in the database
            var actualOferta = context.Oferta.Include(p => p.OfertaSandwich).
                                FirstOrDefault(p => p.Id == expectedOferta.Id);
            Assert.Equal(expectedOferta, actualOferta);

        }
    }
}
