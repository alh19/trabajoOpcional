using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public SelectSandwichesForPurchase_test()
        {
            _contextOptions = UtilitiesForSandwiches.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            UtilitiesForSandwiches.InitializeDbSandwichesForTests(context);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void Select_withoutAlergenoAndPrecio()
        {
            using (context)
            {
                IEnumerable<Sandwich> expectedSandwiches = UtilitiesForSandwiches.GetSandwiches(0, 3).OrderBy(s => s.SandwichName).ToList();
                var controller = new SandwichesController(context);
                SelectSandwichesViewModel expectedModel = new SelectSandwichesViewModel();
                expectedModel.Sandwiches = expectedSandwiches.Select(s=>new SandwichForPurchaseViewModel(s));
                expectedModel.sandwichPrecio = 0;
                expectedModel.sandwichAlergenoSelected = null;
                expectedModel.Alergenos = new SelectList(UtilitiesForSandwiches.GetAlergenos(0, 2).Select(a => a.Name));

                //Act
                var result =  controller.SelectSandwichForPurchase(0, null);

                var viewResult = Assert.IsType<ViewResult>(result);

                SelectSandwichesViewModel viewModel = (result as ViewResult).Model as SelectSandwichesViewModel;

                Assert.Equal(expectedModel, viewModel);
            }
        }

        public static IEnumerable<object[]> TestCasesForSelect_AlergenoSelected()
        {
            UtilitiesForSandwiches.CrearDatos();
            var allTest = new List<object[]>
            {
                new object[]{UtilitiesForSandwiches.GetSandwiches(1,1).ToList(),"Huevo"},
                new object[]{new List<Sandwich> {}, "Leche" }
            };
            UtilitiesForSandwiches.BorrarDatos();
            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelect_AlergenoSelected))]
        [Trait("LevelTesting", "Unit Testing")]
        public void Select_AlergenoSelected(List<Sandwich>expectedModel, string alergeno)
        {
            using (context)
            {
                var controller = new SandwichesController(context);
                var result = controller.SelectSandwichForPurchase(0, alergeno);

                var viewResult = Assert.IsType<ViewResult>(result);

                SelectSandwichesViewModel viewModel = (result as ViewResult).Model as SelectSandwichesViewModel;

                //Assert.Equal(expectedModel, viewModel.Sandwiches);

            }
        }

    }
}
