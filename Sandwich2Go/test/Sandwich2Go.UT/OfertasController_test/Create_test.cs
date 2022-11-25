using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.OfertaViewModels;
using Sandwich2Go.Models.SandwichViewModels;
using Sandwich2Go.UT.SandwichControllers_test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
