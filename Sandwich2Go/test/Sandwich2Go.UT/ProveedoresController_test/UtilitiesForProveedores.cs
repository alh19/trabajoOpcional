using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Sandwich2Go.UT.SandwichControllers_test
{
    public class UtilitiesForProveedores
    {
        static IList<IngrProv> IngrProv;
        static IList<IList<IngrProv>> IngrProvG;
        static IList<Ingrediente> IngredientesG;
        static IList<Proveedor> ProveedoresG;

        public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Crear un nuevo proveedor de servicios, y una nueva
            //instancia de la base de datos temporal:
            var serviceProvider = new ServiceCollection()
            //.AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();
            // Crear una nueva instancia de opciones que use 
            //la BD temporal ofrecida por el proveedor de servicios:
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //builder.UseInMemoryDatabase("Sandwich2Go")
            //.UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }

        public static void InitializeDbProveedoresForTests(ApplicationDbContext db)
        {
            
        }

        public static void ReInitializeDbProveedoresForTests(ApplicationDbContext db)
        {
            db.Sandwich.RemoveRange(db.Sandwich);
            db.IngredienteSandwich.RemoveRange(db.IngredienteSandwich);
            db.Ingrediente.RemoveRange(db.Ingrediente);
            db.Alergeno.RemoveRange(db.Alergeno);
            db.AlergSandws.RemoveRange(db.AlergSandws);

            db.SaveChanges();
        }

        public static void CrearDatos()
        {
        }
        public static void BorrarDatos()
        {
            IngrProv = null;
            IngrProvG = null;
            IngredientesG = null;
            ProveedoresG = null;
        }
    }

}