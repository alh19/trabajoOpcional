using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandwich2Go.Models;
using Sandwich2Go.Models.PedidoViewModels;
using Sandwich2Go.UT.SandwichControllers_test;
using Xunit;
using Sandwich2Go.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Sandwich2Go.UT.PedidoController_test
{
    public class DetailsPedido_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext pedidoContext;
        IList<Pedido> pedidos;

        public DetailsPedido_test()
        {
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();


            UtilitiesForPedidoDetails.InitializeDbPedidosForTests(context);
            pedidos = UtilitiesForPedidoDetails.GetPedidos(0, 1);

            System.Security.Principal.GenericIdentity user = new("gregorio@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new(user);
            pedidoContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            pedidoContext.User = identity;
            ClientePedido();
        }

        public static IEnumerable<object[]> TestCasesFor_Pedido_notfound_withoutId()
        {
            var allTests = new List<object[]>
            {
                new object[]{null},
                new object[]{200}
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_Pedido_notfound_withoutId))]
        [Trait("LevelTesting","Unit Testing")]
        public void Details_Purchase_notfound(int? id)
        {
            using (context)
            {
                //Arrange
                var controller = new PedidosController(context);
                controller.ControllerContext.HttpContext = pedidoContext;

                //Act
                var result = controller.Details(id);

                //Assert
                var viewResult = Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task Details_Pedido_found()
        {
            
            using (context)
            {
                //Arrange
                var expectedPurchase = new PedidoDetailsViewModel(UtilitiesForPedidoDetails.GetPedidos(0, 1).First());

                var controller = new PedidosController(context);
                controller.ControllerContext.HttpContext = pedidoContext;

                //Act
                var result = await controller.Details(UtilitiesForPedidoDetails.GetPedidos(0, 1).First().Id);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);

                var model = viewResult.Model as PedidoDetailsViewModel;

                Assert.Equal(expectedPurchase, model);
            }
        }

        public void ClientePedido()
        {
            Pedido pedido = (context as ApplicationDbContext).Pedido.Where(p => p.Id == 1).First();
            pedido.Cliente = ((context as ApplicationDbContext).Users.Where(u => u.Id=="2").First()) as Cliente;
            (context as ApplicationDbContext).Update(pedido);
            (context as ApplicationDbContext).SaveChanges();
        }
    }
}
