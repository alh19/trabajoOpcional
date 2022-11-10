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
                var controller = new SandwichesController(context);
                var result = controller.SelectSandwichForPurchase(precio, alergeno);

                var viewResult = Assert.IsType<ViewResult>(result);

                SelectSandwichesViewModel viewModel = (result as ViewResult).Model as SelectSandwichesViewModel;

                Assert.Equal(expectedModel, viewModel);

            }
        }

    }
}
