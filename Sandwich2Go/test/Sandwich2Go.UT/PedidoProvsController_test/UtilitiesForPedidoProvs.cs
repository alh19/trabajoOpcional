using Design;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AlergSandw = Sandwich2Go.Models.AlergSandw;
using Gerente = Sandwich2Go.Models.Gerente;
using Ingrediente = Sandwich2Go.Models.Ingrediente;
using IngredienteSandwich = Sandwich2Go.Models.IngredienteSandwich;
using IngrPedProv = Sandwich2Go.Models.IngrPedProv;
using MetodoDePago = Sandwich2Go.Models.MetodoDePago;
using PedidoProv = Sandwich2Go.Models.PedidoProv;
using Tarjeta = Sandwich2Go.Models.Tarjeta;

namespace Sandwich2Go.UT.PedidoProvsController_test
{
    public class UtilitiesForPedidoProvs
    {
        public static void InitializeDbPedidoProvsForTests(ApplicationDbContext db, IList<PedidoProv> pedidos)
        {
            db.PedidoProv.AddRange(pedidos);
            db.IngrPedProv.AddRange(pedidos.Select(p => p.IngrPedProv.ToList()) as IngrPedProv);
            db.SaveChanges();
        }

        public static void InitializeDbPedidoProvsForTests(ApplicationDbContext db)
        {
            var gerente = new Gerente { Id = "1", UserName = "elena@uclm.com", Email = "elena@uclm.com", Nombre = "Elena", Apellido = "Navarro Martinez", EmailConfirmed = true, Direccion = "" };
            db.PedidoProv.AddRange(GetPedidoProvs(0,2, GetIngredientes(0,3), gerente));
            db.SaveChanges();
        }

        public static void ReInitializeDbPedidoProvsForTests(ApplicationDbContext db)
        {
            db.IngrPedProv.RemoveRange(db.IngrPedProv);
            db.PedidoProv.RemoveRange(db.PedidoProv);
            db.SaveChanges();
        }

        public static IList<Ingrediente> GetIngredientes(int index, int numOfIngredientes)
        {
            IList<Ingrediente> ingredientes = GetIngredientes(0, 2);

            var allIngredientes = new List<Ingrediente>
            {
                new Ingrediente { Id = 1, Nombre = "Queso", PrecioUnitario = 1, Stock = 110, AlergSandws = new List<AlergSandw>{ new AlergSandw { AlergenoId=2, IngredienteId=1, Id=2 } }, IngredienteSandwich = new List<IngredienteSandwich>{ new IngredienteSandwich {Id=1, IngredienteId=2, SandwichId=2,Cantidad=1, Ingrediente = ingredientes[0] } } },
                new Ingrediente { Id = 5, Nombre = "Jamon", PrecioUnitario = 1, Stock = 36, AlergSandws = new List<AlergSandw>{ new AlergSandw { AlergenoId=3, IngredienteId=3, Id=3 } }, IngredienteSandwich = new List<IngredienteSandwich>{ new IngredienteSandwich {Id=2, IngredienteId=4, SandwichId=3,Cantidad=1, Ingrediente = ingredientes[1] } } },
                new Ingrediente { Id = 4, Nombre = "Huevo", PrecioUnitario = 2, Stock = 210, AlergSandws = new List<AlergSandw>{ new AlergSandw { AlergenoId=2, IngredienteId=1, Id=2 } }, IngredienteSandwich = new List<IngredienteSandwich>{ new IngredienteSandwich {Id=3, IngredienteId=3, SandwichId=1,Cantidad=2, Ingrediente = ingredientes[2] } } },
            };
            return allIngredientes.GetRange(index, numOfIngredientes);
        }

        public static IList<PedidoProv> GetPedidoProvs(int index, int numOfPurchases,
            IList<Ingrediente> ingredientes, Gerente gerente)
        {
            PedidoProv pedidoprov1 = new PedidoProv(1,
                        7,
                        "Calle 12345",
                        DateTime.Now,
                        gerente,
                        new List<IngrPedProv>()
                        {
                           new IngrPedProv(){Id = 1, Cantidad= 3, PedidoProvId = 1, IngrProvId=1, IngrProv=ingredientes[0].IngrProv.First()}
                        },
                        new Tarjeta { Id = 1, AnoCaducidad = 2030, CCV = 123, MesCaducidad = 12, Numero = 1234123412344321 });

            pedidoprov1.IngrPedProv[0].PedidoProv = pedidoprov1;

            PedidoProv pedidoprov2 = new PedidoProv(1,
                        7,
                        "Calle 12345",
                        DateTime.Now,
                        gerente,
                        new List<IngrPedProv>()
                        {
                           new IngrPedProv(){Id = 1, Cantidad= 3, PedidoProvId = 1, IngrProvId=1, IngrProv=ingredientes[0].IngrProv.First()}
                        },
                        new Tarjeta { Id = 1, AnoCaducidad = 2030, CCV = 123, MesCaducidad = 12, Numero = 1234123412344321 });

            pedidoprov2.IngrPedProv[0].PedidoProv = pedidoprov2;

            List<PedidoProv> allPedidos = new List<PedidoProv> { pedidoprov1, pedidoprov2 };

            return allPedidos.GetRange(index, numOfPurchases);

        }


    }
}

