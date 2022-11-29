using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.OfertaViewModels;
using Sandwich2Go.UT.SandwichControllers_test;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sandwich2Go.UT.OfertasController_test
{
    public class Details_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext ofertaContext;
        IList<Oferta> ofertas;

        public Details_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Gerente gerente = Utilities.GetUsers(0, 1).First() as Gerente;
            ofertas = UtilitiesForOfertas.GetOfertas(0, 2,
                UtilitiesForSandwichesForOffer.GetSandwiches(0, 3), gerente);
            UtilitiesForOfertas.InitializeDbOfertasForTests(context, ofertas);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            ofertaContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            ofertaContext.User = identity;

        }


        public static IEnumerable<object[]> TestCasesFor_Oferta_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null },
                new object[] {100},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Oferta_notfound_withoutId))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Details_Oferta_notfound(int? id)
        {
            // Arrange
            using (context)
            {
                var controller = new OfertasController(context);
                controller.ControllerContext.HttpContext = ofertaContext;


                // Act
                var result = controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result.Result);

            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Details_Purchase_found()
        {
            // Arrange
            var expectedOferta = new OfertaDetailsViewModel(ofertas.First());
            var controller = new OfertasController(context);
            controller.ControllerContext.HttpContext = ofertaContext;

            // Act
            var result = controller.Details(expectedOferta.Id);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);

            var model = viewResult.Model as OfertaDetailsViewModel;
            Assert.Equal(expectedOferta, model);
        }
    }
}
