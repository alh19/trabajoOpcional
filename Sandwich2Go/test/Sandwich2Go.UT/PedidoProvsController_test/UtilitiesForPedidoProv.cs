using System;
using System.Collections.Generic;
using Sandwich2Go.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sandwich2Go.Data;
using Design;
using Ingrediente = Sandwich2Go.Models.Ingrediente;
using MetodoDePago = Sandwich2Go.Models.MetodoDePago;
using IngrPedProv = Sandwich2Go.Models.IngrPedProv;
using Tarjeta = Sandwich2Go.Models.Tarjeta;
using Efectivo = Sandwich2Go.Models.Efectivo;
using AlergSandw = Sandwich2Go.Models.AlergSandw;
using Gerente = Sandwich2Go.Models.Gerente;
using IngrProv = Sandwich2Go.Models.IngrProv;
using Alergeno = Sandwich2Go.Models.Alergeno;
using PedidoProv = Sandwich2Go.Models.PedidoProv;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Proveedor = Sandwich2Go.Models.Proveedor;

namespace Sandwich2Go.UT.PedidoProvsController_test
{
    public class UtilitiesForPedidoProv
    {
        static IList<PedidoProv> PedidoProvsG;
        static IList<Ingrediente> IngredientesG;
        static IList<Proveedor> ProveedoresG;
        static IList<MetodoDePago> MetodosDePagoG;
        static IList<IngrPedProv> IngrPedProvG;
        static IList<Alergeno> AlergenosG;
        static IList<IList<AlergSandw>> AlergSandwsG;
        static IList<IList<IngrProv>> IngrProvG;
        static Gerente GerenteG;

        public static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
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

        //Getters
        public static IList<Ingrediente> GetIngredientes(int index, int numOfSandwiches)
        {
            return IngredientesG.ToList().GetRange(index, numOfSandwiches);
        }

        public static IList<Alergeno> GetAlergenos(int index, int numOfAlergenos)
        {
            return AlergenosG.ToList().GetRange(index, numOfAlergenos);
        }

        public static IList<Proveedor> GetProveedores(int index, int numOfAlergenos)
        {
            return ProveedoresG.ToList().GetRange(index, numOfAlergenos);
        }

        public static IList<MetodoDePago> GetMetodosDePago(int index, int numOfMetodosDePago)
        {
            return MetodosDePagoG.ToList().GetRange(index, numOfMetodosDePago);
        }

        public static IList<IList<AlergSandw>> GetAlergSandw(int index, int numOfAlergSandw)
        {
            return AlergSandwsG.ToList().GetRange(index, numOfAlergSandw);
        }

        public static IList<IList<IngrProv>> GetIngrProv(int index, int numOfSandwiches)
        {
            return IngrProvG.ToList().GetRange(index, numOfSandwiches);
        }

        public static Gerente GetGerente(int index, int numOfUser)
        {
            return GerenteG = Utilities.GetUsers(index, numOfUser).First() as Gerente;
        }

        //Initialize
        public static void InitializeDbPedidoProvsForTests(ApplicationDbContext db)
        {
            BorrarDatos();
            CrearDatos();
            ReInitializeDbPedidosForTests(db);
            db.Alergeno.AddRange(GetAlergenos(0, 2));
            db.Ingrediente.AddRange(GetIngredientes(0, 4));
            db.Users.AddRange(Utilities.GetUsers(0, 2));
            db.MetodoDePago.AddRange(GetMetodosDePago(0, 2));

            
            IList<IList<AlergSandw>> alergSandw = GetAlergSandw(0, 3);
            while (!(alergSandw.Count() == 0))
            {

                db.AlergSandws.AddRange(alergSandw.ElementAt(0));
                alergSandw.RemoveAt(0);
            }

            db.SaveChanges();
        }


        //Métodos
        public static void CrearDatos()
        {
            IngredientesG = new List<Ingrediente>();
            MetodosDePagoG = new List<MetodoDePago>();
            IngrPedProvG = new List<IngrPedProv>();
            AlergenosG = new List<Alergeno>();
            AlergSandwsG = new List<IList<AlergSandw>>();
            IngrProvG = new List<IList<IngrProv>>();

            MetodosDePagoG.Add(new Tarjeta { Id = 1, AnoCaducidad = 2030, CCV = 123, MesCaducidad = 12, Numero = 1234123412344321 });
            MetodosDePagoG.Add(new Efectivo { Id = 2, NecesitasCambio = true });


            IngredientesG.Add(new Ingrediente { Id = 1, Nombre = "Jamon", Stock = 100 });
            IngredientesG.Add(new Ingrediente { Id = 2, Nombre = "Queso", Stock = 100 });
            IngredientesG.Add(new Ingrediente { Id = 3, Nombre = "Huevo", Stock = 100 });
            IngredientesG.Add(new Ingrediente { Id = 4, Nombre = "Mayonesa", Stock = 100 });

            ProveedoresG.Add(new Proveedor { Id = 1, Cif = "1111a", Nombre="Alberto", Direccion="Calle 1", 
                IngrProv = new List<IngrProv> { 
                    new IngrProv
                    {
                        Id = 2,
                        IngredienteId = 1,
                        ProveedorId = 1,
                        IngrPedProvs = new List<IngrPedProv> {
                            new IngrPedProv
                            {
                                Id = 1,
                                IngrProvId = 1,
                                PedidoProvId = 1,
                                Cantidad = 4
                            }
                        
                        }
                    }
                }
            });

            ProveedoresG.Add(new Proveedor
            {
                Id = 2,
                Cif = "2222a",
                Nombre = "Ana",
                Direccion = "Calle 2",
                IngrProv = new List<IngrProv> {
                    new IngrProv
                    {
                        Id = 3,
                        IngredienteId = 2,
                        ProveedorId = 2,
                        IngrPedProvs = new List<IngrPedProv> {
                            new IngrPedProv
                            {
                                Id = 2,
                                IngrProvId = 2,
                                PedidoProvId = 2,
                                Cantidad = 3
                            }

                        }
                    }
                }
            });

            ProveedoresG.Add(new Proveedor
            {
                Id = 1,
                Cif = "3333c",
                Nombre = "Rogelio",
                Direccion = "Calle 3",
                IngrProv = new List<IngrProv> {
                    new IngrProv
                    {
                        Id = 3,
                        IngredienteId = 3,
                        ProveedorId = 4,
                        IngrPedProvs = new List<IngrPedProv> {
                            new IngrPedProv
                            {
                                Id = 3,
                                IngrProvId = 3,
                                PedidoProvId = 3,
                                Cantidad = 3
                            }

                        }
                    }
                }
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

            PedidoProvsG.Add(new PedidoProv
            {
                Id = 1,
                PrecioTotal = 4.95,
                DireccionEnvio = "Calle Restaurante Sandwich",
                FechaPedido = System.DateTime.Now,
                Gerente = GetGerente(0,0),
                MetodoDePago = MetodosDePagoG[0],
                IngrPedProv = new List<IngrPedProv> {
                    new IngrPedProv
                    {
                        Id = 1,
                        IngrProvId = 1,
                        PedidoProvId = 1,
                        Cantidad = 4
                    }
                }
            });

            PedidoProvsG.Add(new PedidoProv
            {
                Id = 1,
                PrecioTotal = 9,
                DireccionEnvio = "Calle Restaurante Sandwich",
                FechaPedido = System.DateTime.Now,
                Gerente = GetGerente(0, 0),
                MetodoDePago = MetodosDePagoG[1],
                IngrPedProv = new List<IngrPedProv> {
                    new IngrPedProv
                    {
                        Id = 2,
                        IngrProvId = 2,
                        PedidoProvId = 2,
                        Cantidad = 7
                    }
                }
            });

        }

        public static void BorrarDatos()
        {
            IngredientesG = null;
            MetodosDePagoG = null;
            IngrPedProvG = null;
            AlergenosG = null;
            AlergSandwsG = null;
            GerenteG = null;
            IngrProvG = null;
            ProveedoresG = null;
        }

        public static void ReInitializeDbPedidosForTests(ApplicationDbContext db)
        {
            db.Sandwich.RemoveRange(db.Sandwich);
            db.IngredienteSandwich.RemoveRange(db.IngredienteSandwich);
            db.Ingrediente.RemoveRange(db.Ingrediente);
            db.Alergeno.RemoveRange(db.Alergeno);
            db.AlergSandws.RemoveRange(db.AlergSandws);
            db.Oferta.RemoveRange(db.Oferta);
            db.MetodoDePago.RemoveRange(db.MetodoDePago);
            db.OfertaSandwich.RemoveRange(db.OfertaSandwich);
            db.Proveedor.RemoveRange(db.Proveedor);
            db.SandwichPedido.RemoveRange(db.SandwichPedido);
            db.Users.RemoveRange(db.Users);
            db.Pedido.RemoveRange(db.Pedido);

            db.SaveChanges();
        }
    }
}
