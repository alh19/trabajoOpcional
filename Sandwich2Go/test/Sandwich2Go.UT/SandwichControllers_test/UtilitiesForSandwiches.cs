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
    public class UtilitiesForSandwiches
    {
        static IList<Alergeno> AlergenosG;
        static IList<IList<AlergSandw>> AlergSandwsG;
        static IList<Ingrediente> IngredientesG;
        static IList<IList<IngredienteSandwich>> IngredienteSandwichesG;
        static IList<Sandwich> SandwichesG;
        static IList<Oferta> OfertasG;
        static IList<IList<OfertaSandwich>> OfertaSandwichesG;

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
            return SandwichesG.ToList().GetRange(index, numOfSandwiches);
        }

        public static void InitializeDbSandwichesForTests(ApplicationDbContext db)
        {
            CrearDatos();
            db.Alergeno.AddRange(GetAlergenos(0,2));
            db.Ingrediente.AddRange(GetIngredientes(0,4));
            db.Sandwich.AddRange(GetSandwiches(0,3));
            db.Users.AddRange(Utilities.GetUsers(0, 2));

            IList<IList<IngredienteSandwich>> ingredienteSandwiches = GetIngredienteSandwich(0,3);

            while (!(ingredienteSandwiches.Count() == 0))
            {

                db.IngredienteSandwich.AddRange(ingredienteSandwiches.ElementAt(0));
                ingredienteSandwiches.RemoveAt(0);
            }
            IList<IList<AlergSandw>> alergSandw = GetAlergSandw(0,3);
            while (!(alergSandw.Count() == 0))
            {

                db.AlergSandws.AddRange(alergSandw.ElementAt(0));
                alergSandw.RemoveAt(0);
            }

            IList<IList<OfertaSandwich>> ofertaSandwiches = GetOfertaSandwiches(0, 1);
            while (!(ofertaSandwiches.Count() == 0))
            {

                db.OfertaSandwich.AddRange(ofertaSandwiches.ElementAt(0));
                ofertaSandwiches.RemoveAt(0);
            }
            db.SaveChanges();
        }

        public static void ReInitializeDbSandwichesForTests(ApplicationDbContext db)
        {
            db.Sandwich.RemoveRange(db.Sandwich);
            db.IngredienteSandwich.RemoveRange(db.IngredienteSandwich);
            db.Ingrediente.RemoveRange(db.Ingrediente);
            db.Alergeno.RemoveRange(db.Alergeno);
            db.AlergSandws.RemoveRange(db.AlergSandws);
            db.Oferta.RemoveRange(db.Oferta);
            db.OfertaSandwich.RemoveRange(db.OfertaSandwich);
            db.Users.RemoveRange(db.Users);

            db.SaveChanges();
        }
        public static IList<Ingrediente> GetIngredientes(int index, int numOfIngredientes)
        {
            return IngredientesG.ToList().GetRange(index, numOfIngredientes);
        }
        public static IList<IList<AlergSandw>> GetAlergSandw(int index, int numOfAlergSandw)
        {
            return AlergSandwsG.ToList().GetRange(index, numOfAlergSandw);
        }

        public static IList<Oferta> GetOfertas(int index, int numOfIngredientes)
        {
            return OfertasG.ToList().GetRange(index, numOfIngredientes);
        }
        public static IList<IList<OfertaSandwich>> GetOfertaSandwiches(int index, int numOfAlergSandw)
        {
            return OfertaSandwichesG.ToList().GetRange(index, numOfAlergSandw);
        }


        public static IList<IList<IngredienteSandwich>> GetIngredienteSandwich(int index, int numOfIngredienteSandwich)
        {
            return IngredienteSandwichesG.ToList().GetRange(index, numOfIngredienteSandwich); ;
        }

        public static IList<Alergeno> GetAlergenos(int index, int numOfAlergenos)
        {
            return AlergenosG.ToList().GetRange(index,numOfAlergenos);
        }

        public static void CrearDatos()
        {
            AlergenosG = new List<Alergeno>();
            IngredientesG = new List<Ingrediente>();
            SandwichesG = new List<Sandwich>();
            IngredienteSandwichesG = new List<IList<IngredienteSandwich>>();
            AlergSandwsG = new List<IList<AlergSandw>>();
            OfertasG = new List<Oferta>();
            OfertaSandwichesG = new List<IList<OfertaSandwich>>();

            AlergenosG.Add(new Alergeno { id = 1, Name = "Huevo" });
            AlergenosG.Add(new Alergeno { id = 2, Name = "Leche" });

            IngredientesG.Add(new Ingrediente { Id = 1, Nombre = "Jamon" , Stock = 100});
            IngredientesG.Add(new Ingrediente { Id = 2, Nombre = "Queso" , Stock = 100});
            IngredientesG.Add(new Ingrediente { Id = 3, Nombre = "Huevo" , Stock = 100});
            IngredientesG.Add(new Ingrediente { Id = 4, Nombre = "Mayonesa" , Stock = 100 });

            SandwichesG.Add(new Sandwich{Id = 1, SandwichName = "Cubano", Precio = 5.50, Desc = "Queso, jamón y mayonesa", OfertaSandwich = new List<OfertaSandwich>() });
            SandwichesG.Add(new Sandwich{Id = 2, SandwichName = "Mixto", Precio = 3.00, Desc = "Jamón y queso", OfertaSandwich = new List<OfertaSandwich>() });
            SandwichesG.Add(new Sandwich{Id = 3,SandwichName = "Inglés", Precio = 4.00, Desc = "Jamón, queso y huevo revuelto", OfertaSandwich = new List<OfertaSandwich>() });

            OfertaSandwichesG.Add(new List<OfertaSandwich> { new OfertaSandwich { OfertaId = 1, SandwichId = 1, Sandwich = SandwichesG[0],Porcentaje = 10 } });

            OfertasG.Add(new Oferta { Id = 1, Nombre = "Oferta 1", Descripcion ="Desc Oferta 1", FechaFin=new DateTime(), FechaInicio= new DateTime(), OfertaSandwich = OfertaSandwichesG[0] });

            OfertaSandwichesG[0][0].Oferta = OfertasG[0];

            SandwichesG[0].OfertaSandwich = OfertaSandwichesG[0];

            IngredienteSandwichesG.Add(new List<IngredienteSandwich> {
                new IngredienteSandwich{ Id = 1, Ingrediente = IngredientesG[0], Sandwich = SandwichesG[0], IngredienteId = IngredientesG[0].Id, SandwichId = SandwichesG[0].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 2, Ingrediente = IngredientesG[1], Sandwich = SandwichesG[0], IngredienteId = IngredientesG[1].Id, SandwichId = SandwichesG[0].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 3, Ingrediente = IngredientesG[3], Sandwich = SandwichesG[0], IngredienteId = IngredientesG[3].Id, SandwichId = SandwichesG[0].Id, Cantidad = 1}
            });
            IngredienteSandwichesG.Add(
            new List<IngredienteSandwich> {
                new IngredienteSandwich{ Id = 4, Ingrediente = IngredientesG[0], Sandwich = SandwichesG[1], IngredienteId = IngredientesG[0].Id, SandwichId = SandwichesG[1].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 5, Ingrediente = IngredientesG[1], Sandwich = SandwichesG[1], IngredienteId = IngredientesG[1].Id, SandwichId = SandwichesG[1].Id, Cantidad = 1},
            });
            IngredienteSandwichesG.Add(
            new List<IngredienteSandwich> {
                new IngredienteSandwich{ Id = 6, Ingrediente = IngredientesG[0], Sandwich = SandwichesG[2], IngredienteId = IngredientesG[0].Id, SandwichId = SandwichesG[2].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 7, Ingrediente = IngredientesG[1], Sandwich = SandwichesG[2], IngredienteId = IngredientesG[1].Id, SandwichId = SandwichesG[2].Id, Cantidad = 1},
                new IngredienteSandwich{ Id = 8, Ingrediente = IngredientesG[2], Sandwich = SandwichesG[2], IngredienteId = IngredientesG[2].Id, SandwichId = SandwichesG[2].Id, Cantidad = 1}
            });

            AlergSandwsG.Add(new List<AlergSandw> {
                new AlergSandw{Id = 1, Ingrediente = IngredientesG[1], Alergeno = AlergenosG[1], AlergenoId = AlergenosG[1].id, IngredienteId = IngredientesG[1].Id}
            });
            AlergSandwsG.Add(new List<AlergSandw> {
                new AlergSandw{Id = 2, Ingrediente = IngredientesG[2], Alergeno = AlergenosG[0], AlergenoId = AlergenosG[0].id, IngredienteId = IngredientesG[2].Id}
            });
            AlergSandwsG.Add(new List<AlergSandw> {
                new AlergSandw{Id = 3, Ingrediente = IngredientesG[3], Alergeno = AlergenosG[1], AlergenoId = AlergenosG[1].id, IngredienteId = IngredientesG[3].Id}
            });

            AlergenosG[0].AlergSandws = new List<AlergSandw> { AlergSandwsG[1][0], AlergSandwsG[2][0] };
            AlergenosG[1].AlergSandws = new List<AlergSandw> { AlergSandwsG[0][0] };

            IngredientesG[0].AlergSandws = new List<AlergSandw> { };
            IngredientesG[1].AlergSandws = AlergSandwsG[0];
            IngredientesG[2].AlergSandws = AlergSandwsG[1];
            IngredientesG[3].AlergSandws = AlergSandwsG[2];

            IngredientesG[0].IngredienteSandwich = new List<IngredienteSandwich> { IngredienteSandwichesG[0][0], IngredienteSandwichesG[1][0], IngredienteSandwichesG[2][0] };
            IngredientesG[1].IngredienteSandwich = new List<IngredienteSandwich> { IngredienteSandwichesG[0][1], IngredienteSandwichesG[1][1], IngredienteSandwichesG[2][1] };
            IngredientesG[2].IngredienteSandwich = new List<IngredienteSandwich> { IngredienteSandwichesG[2][2] };
            IngredientesG[3].IngredienteSandwich = new List<IngredienteSandwich> { IngredienteSandwichesG[0][2] };

            SandwichesG[0].IngredienteSandwich = IngredienteSandwichesG[0];
            SandwichesG[1].IngredienteSandwich = IngredienteSandwichesG[1];
            SandwichesG[2].IngredienteSandwich = IngredienteSandwichesG[2];
        }
        public static void BorrarDatos()
        {
            AlergenosG = null;
            IngredientesG = null;
            SandwichesG = null;
            IngredienteSandwichesG = null;
            AlergSandwsG = null;
        }
    }

}

