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
using Sandwich2Go.Models.IngredienteViewModels;
using Sandwich2Go.Models.ProveedorViewModels;
using Sandwich2Go.UT.IngredientesController_test;
using Sandwich2Go.UT.ProveedoresController_test;
using Sandwich2Go.UT.SandwichControllers_test;
using Xunit;

namespace Sandwich2Go.UT.ProveedoresController_test
{
    public class SelectProveedores_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext ingredienteContext;

        public SelectProveedores_test()
        {
            //Initialize the Database
            _contextOptions = Utilities.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the InMemory database with test data.
            UtilitiesForProveedores.InitializeDbProveedoresForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("elena@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            ingredienteContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            ingredienteContext.User = identity;

        }

        public static IEnumerable<object[]> TestCasesForSelectProveedoresForPurchase_get()
        {
            var alergenosNames = new SelectList(UtilitiesForIngredientes.GetAlergenos(0, 4).Select(a => a.Name));
            var ingredientesForPurchaseVMTC1 = new SelectIngredientesForPurchaseViewModel()
            {
                Alergenos = alergenosNames,
                Ingredientes = UtilitiesForIngredientes.GetIngredientes(0, 2)
                        .OrderBy(i => i.Id)
                        .Select(h => new IngredienteForPurchaseViewModel(h)).ToList()
            };

            var ingredientesForPurchaseVMTC2 = new SelectIngredientesForPurchaseViewModel()
            {
                Alergenos = alergenosNames,
                Ingredientes = UtilitiesForIngredientes.GetIngredientes(0, 2)
                        .OrderBy(i => i.Id)
                        .Select(h => new IngredienteForPurchaseViewModel(h)).ToList()
            };

            var ingredientesForPurchaseVMTC3 = new SelectIngredientesForPurchaseViewModel()
            {
                Alergenos = alergenosNames,
                Ingredientes = UtilitiesForIngredientes.GetIngredientes(0, 2)
                        .OrderBy(i => i.Id)
                        .Select(h => new IngredienteForPurchaseViewModel(h)).ToList()
            };
            var allTests = new List<object[]>
                {
                    new object[] { ingredientesForPurchaseVMTC1, null, null},
                    new object[] { ingredientesForPurchaseVMTC2, "Gluten", null},
                    new object[]{ ingredientesForPurchaseVMTC3, null,"Lechuga" },
                  };
            return allTests;
        }
    }
}
