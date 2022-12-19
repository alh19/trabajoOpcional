using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.IngredienteViewModels;
using Sandwich2Go.Models.OfertaViewModels;
using Sandwich2Go.Models.PedidoProvViewModels;
using Sandwich2Go.Models.SandwichViewModels;
using Sandwich2Go.UT.SandwichControllers_test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Gerente = Sandwich2Go.Models.Gerente;
using Ingrediente = Sandwich2Go.Models.Ingrediente;

namespace Sandwich2Go.UT.PedidoProvsController_test
{
    public class CreatePedidoProvs_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext httpContext;
        IList<Ingrediente> ingredientes;
        Gerente gerente;

        public CreatePedidoProvs_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();
            UtilitiesForPedidoProvs.InitializeDbPedidoProvsForTests(context);
            ingredientes = context.Ingrediente.ToList();
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

        //[Fact]
        //[Trait("LevelTesting", "Unit Testing")]
        //public void Create_Get_WithSelectedIngredientes()
        //{
        //    // Arrange

        //    var controller = new PedidoProvsController(context);
        //    //simulate user's connection
        //    controller.ControllerContext.HttpContext = httpContext;


        //    SelectedIngrProvForPurchaseViewModel sandwichesVM = new() { IdsToAdd = new string[1] { "1" } };

        //    IngrProvForPurchaseViewModel ingrpedprovViewModel = new IngrProvForPurchaseViewModel(ingredientes.First());
        //    PedidoProvCreateViewModel expectedOfertaVM = new PedidoProvCreateViewModel(gerente, new IngrProvForPurchaseViewModel[1] { ingrpedprovViewModel });


        //    // Act
        //    var result = controller.Create(sandwichesVM);

        //    //Assert
        //    ViewResult viewResult = Assert.IsType<ViewResult>(result.Result);
        //    PedidoProvCreateViewModel currentOferta = viewResult.Model as PedidoProvCreateViewModel;

        //    Assert.Equal(expectedOfertaVM, currentOferta);
        //}
    }
    
}
