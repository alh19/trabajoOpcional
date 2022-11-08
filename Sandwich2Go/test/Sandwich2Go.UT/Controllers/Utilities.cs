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

namespace Sandwich2Go.UT.Controllers
{
    public class Utilities
    {
        public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions(){
            // Crear un nuevo proveedor de servicios, y una nueva
            //instancia de la base de datos temporal:
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();
            // Crear una nueva instancia de opciones que use 
            //la BD temporal ofrecida por el proveedor de servicios:
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("Sandwich2Go")
            .UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }

        public static IList<Sandwich> GetSandwiches(int index, int numOfSandwiches)
        {
            IList<IngredienteSandwich> ingredientes = GetIngredienteSandwich(0, 1).First();
            var allSandwiches = new List<Sandwich>{
                new Sandwich { Id = 1, SandwichName = "Cubano", IngredienteSandwich= ingredientes,
                Precio = 5.50, Desc = "Queso, jamón y mayonesa" },

                new Sandwich {Id = 2, SandwichName = "Mixto", IngredienteSandwich= ingredientes,
                Precio = 3.00, Desc = "Jamón y queso" },

                new Sandwich { Id = 3, SandwichName = "Inglés", IngredienteSandwich= ingredientes,
                Precio = 4.00, Desc = "Jamón, queso y huevo revuelto" }
                };
            return allSandwiches.GetRange(index, numOfSandwiches);
        }

        public static void InitializeDbSandwichesForTests(ApplicationDbContext db)
        {
            db.Ingrediente.AddRange(GetIngredientes(0, 4));
            db.Sandwich.AddRange(GetSandwiches(0, 3));
            db.Alergeno.AddRange(GetAlergenos(0, 2));
            IList<IList<IngredienteSandwich>> ingredienteSandwiches = GetIngredienteSandwich(0, 4);
            while (! (ingredienteSandwiches.Count()==0))
            {

                db.IngredienteSandwich.AddRange(ingredienteSandwiches.ElementAt(0));
                ingredienteSandwiches.RemoveAt(0);
            }
            IList<IList<AlergSandw>> alergSandw = GetAlergSandw(0,2);
            while (!(alergSandw.Count() == 0))
            {

                db.AlergSandws.AddRange(alergSandw.ElementAt(0));
                alergSandw.RemoveAt(0);
            }
            db.SaveChanges();
        }
        public static void ReInitializeDbSandwichesForTests(ApplicationDbContext db)
        {
            db.Sandwich.RemoveRange(db.Sandwich);
            db.IngredienteSandwich.RemoveRange(db.IngredienteSandwich);
            db.Ingrediente.RemoveRange(db.Ingrediente);
            db.Alergeno.RemoveRange(db.Alergeno);
            db.SaveChanges();
        }
        public static IList<Ingrediente> GetIngredientes(int index, int numOfIngredientes)
        {
            IList<IList<AlergSandw>> alergenos = GetAlergSandw(0, 4);
            IList<IList<IngredienteSandwich>> ingredientes = GetIngredienteSandwich(0,4);
            var allIngredientes = new List<Ingrediente>{
                new Ingrediente { Id = 1, Nombre = "Jamon", AlergSandws= alergenos[0],
                PrecioUnitario = 1, Stock = 100, IngredienteSandwich = ingredientes[0]},

                new Ingrediente {Id = 2, Nombre = "Queso", AlergSandws= alergenos[1],
                PrecioUnitario = 2, Stock = 100, IngredienteSandwich = ingredientes[1]},

                new Ingrediente { Id = 3, Nombre = "Huevo", AlergSandws= alergenos[2],
                PrecioUnitario = 3, Stock = 100, IngredienteSandwich = ingredientes[2]},
                
                new Ingrediente {Id = 4, Nombre = "Mayonesa", AlergSandws= alergenos[3],
                PrecioUnitario = 4, Stock = 100, IngredienteSandwich = ingredientes[3]}
                };
            return allIngredientes.GetRange(index, numOfIngredientes);
        }
        public static IList<IList<AlergSandw>> GetAlergSandw(int index, int numOfAlergSandw)
        {
            IList<Alergeno> alergenos = GetAlergenos(0,2);
            IList<Ingrediente> ingredientes = GetIngredientes(0, 4);
            var allAlergSandw = new List<IList<AlergSandw>>();
            allAlergSandw.Add(new List<AlergSandw> { 
                new AlergSandw{Id = 1, Ingrediente = ingredientes[1], Alergeno = alergenos[1], AlergenoId = alergenos[1].id, IngredienteId = ingredientes[1].Id}
            });
            allAlergSandw.Add(new List<AlergSandw> {
                new AlergSandw{Id = 2, Ingrediente = ingredientes[2], Alergeno = alergenos[0], AlergenoId = alergenos[0].id, IngredienteId = ingredientes[2].Id}
            });
            allAlergSandw.Add(new List<AlergSandw> {
                new AlergSandw{Id = 3, Ingrediente = ingredientes[3], Alergeno = alergenos[1], AlergenoId = alergenos[1].id, IngredienteId = ingredientes[3].Id}
            });
            return allAlergSandw.GetRange(index, numOfAlergSandw);
        }

        public static IList<IList<IngredienteSandwich>> GetIngredienteSandwich(int index, int numOfIngredienteSandwich)
        {
            IList<Ingrediente> ingredientes = GetIngredientes(0, 4);
            IList<Sandwich> sandwiches = GetSandwiches(0, 3);
            var allIngredienteSandwich = new List<IList<IngredienteSandwich>>();
            allIngredienteSandwich.Add(
            new List<IngredienteSandwich> { 
                new IngredienteSandwich{ Id = 1, Ingrediente = ingredientes[0], Sandwich = sandwiches[0], IngredienteId = ingredientes[0].Id, SandwichId = sandwiches[0].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 2, Ingrediente = ingredientes[1], Sandwich = sandwiches[0], IngredienteId = ingredientes[1].Id, SandwichId = sandwiches[0].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 3, Ingrediente = ingredientes[3], Sandwich = sandwiches[0], IngredienteId = ingredientes[3].Id, SandwichId = sandwiches[0].Id, Cantidad = 1}
            });
            allIngredienteSandwich.Add(
            new List<IngredienteSandwich> {
                new IngredienteSandwich{ Id = 4, Ingrediente = ingredientes[0], Sandwich = sandwiches[1], IngredienteId = ingredientes[0].Id, SandwichId = sandwiches[1].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 5, Ingrediente = ingredientes[1], Sandwich = sandwiches[1], IngredienteId = ingredientes[1].Id, SandwichId = sandwiches[1].Id, Cantidad = 1},
            });
            allIngredienteSandwich.Add(
            new List<IngredienteSandwich> {
                new IngredienteSandwich{ Id = 6, Ingrediente = ingredientes[0], Sandwich = sandwiches[2], IngredienteId = ingredientes[0].Id, SandwichId = sandwiches[2].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 7, Ingrediente = ingredientes[1], Sandwich = sandwiches[2], IngredienteId = ingredientes[1].Id, SandwichId = sandwiches[2].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 8, Ingrediente = ingredientes[2], Sandwich = sandwiches[2], IngredienteId = ingredientes[2].Id, SandwichId = sandwiches[2].Id, Cantidad = 1}
            });
            return allIngredienteSandwich.GetRange(index, numOfIngredienteSandwich); ;
        }

        public static IList<Alergeno> GetAlergenos(int index, int numOfAlergenos)
        {
            IList<IList<AlergSandw>> alergSandws = GetAlergSandw(0,2);
            var allAlergenos = new List<Alergeno>
            {
                new Alergeno{id = 1, Name = "Huevo", AlergSandws = new List<AlergSandw>{ alergSandws[1][0], alergSandws[2][0] } },
                new Alergeno{id = 2, Name = "Leche", AlergSandws =alergSandws[0]}
            };
            return allAlergenos.GetRange(index,numOfAlergenos);
        }
    }

}

