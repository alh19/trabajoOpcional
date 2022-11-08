using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public SelectSandwichesForPurchase_test()
        {
            _contextOptions = UtilitiesForSandwiches.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            UtilitiesForSandwiches.InitializeDbSandwichesForTests(context);

        }

        [Fact]
        public async Task Select_withoutAlergenoAndPrecio()
        {
            using (context)
            {
                IEnumerable<Sandwich> expectedModel = UtilitiesForSandwiches.GetSandwiches(0, 3).OrderBy(s => s.SandwichName).ToList();
                var controller = new SandwichesController(context);

                //Act
                var result =  controller.SelectSandwichForPurchase(0.0, null);

                var viewResult = Assert.IsType<ViewResult>(result);

                List<Sandwich> model = (result as ViewResult).Model as List<Sandwich>;

                Assert.Equal(expectedModel, model);
            }
        }
    }
}
