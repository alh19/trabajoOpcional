using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandwich2Go.Models;
using Sandwich2Go.Models.PedidoProvViewModels;
using Design;
using PedidoProv = Sandwich2Go.Models.PedidoProv;
using Gerente = Sandwich2Go.Models.Gerente;
using Microsoft.AspNetCore.Mvc;
using Sandwich2Go.Controllers;
using Xunit;

namespace Sandwich2Go.UT.PedidoProvsController_test
{
    public class DetailsPedidoProvs_test
    {

        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext pedidoProvContext;
        IList<PedidoProv> pedidos;

        public DetailsPedidoProvs_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the database with test data.
            Gerente gerente = Utilities.GetUsers(0, 1).First() as Gerente;
            pedidos = UtilitiesForPedidoProvs.GetPedidoProvs(0, 2, 
                UtilitiesForPedidoProvs.GetIngredientes(0,3), gerente);
            UtilitiesForPedidoProvs.InitializeDbPedidoProvsForTests(context, pedidos);


            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            pedidoProvContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            pedidoProvContext.User = identity;
        }

        public static IEnumerable<object[]> TestCasesFor_PedidoProv_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[] {null},
                new object[] {100},
                new object[] {1}
            };

            return allTests;
        }

        //[Theory]
        //[MemberData(nameof(TestCasesFor_PedidoProv_notfound_withoutId))]
        //[Trait("LevelTesting", "Unit Testing")]
        //public void Details_PedidoProv_notfound(int? id)
        //{
        //    // Arrange
        //    using (context)
        //    {
        //        var controller = new PedidoProvsController(context);
        //        controller.ControllerContext.HttpContext = pedidoProvContext;

        //        // Act
        //        var result = controller.Details(id);

        //        //Assert
        //        var viewResult = Assert.IsType<NotFoundResult>(result.Result);

        //    }
        //}

        //[Fact]
        //[Trait("LevelTesting", "Unit Testing")]
        //public void Details_Purchase_found()
        //{
        //    // Arrange
        //    var expectedIngr = new PedidoProvDetailsViewModel(pedidos.First().IngrPedProv.First().IngrProv);
        //    var controller = new PedidoProvsController(context);
        //    controller.ControllerContext.HttpContext = pedidoProvContext;

        //    // Act
        //    var result = controller.Details(expectedIngr.Id);

        //    //Assert
        //    var viewResult = Assert.IsType<ViewResult>(result.Result);

        //    var model = viewResult.Model as PedidoProvDetailsViewModel;
        //    Assert.Equal(expectedIngr, model);
        //}

    }
}
