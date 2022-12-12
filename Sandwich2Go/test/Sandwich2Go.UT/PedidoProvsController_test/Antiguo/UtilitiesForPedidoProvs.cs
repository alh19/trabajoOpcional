using Sandwich2Go.Data;
using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandwich2Go.UT.PedidoProvsController_test.Antiguo
{
    internal class UtilitiesForPedidoProvs
    {

        public static void InitializeDbCPedidosProvsForTests(ApplicationDbContext db, IList<PedidoProv> pedidoprov)
        {
            db.PedidoProv.AddRange(pedidoprov);
            db.SaveChanges();
        }

        public static IList<PedidoProv> GetPedidoProvs(int id, int numOfPurchases, IList<MetodoDePago> allPaymentMethods,
            IList<Ingrediente> ingr, Gerente gerente)
        {
            List<IngrProv> ingrprov = new List<IngrProv>();
            double precioTotal = 0;

            var aux = 1;
            foreach (var d in ingr)
            {
                ingrprov.Add(new IngrProv()
                {
                    IngrPedProvs = d.IngrProv.First().IngrPedProvs
                });
                precioTotal += d.IngrProv.First().IngrPedProvs.First().PedidoProv.PrecioTotal;
                aux++;
            }

            DateTime now = DateTime.Now;


            PedidoProv pedidoprov1 = new PedidoProv(1, precioTotal, "Calle Restaurante Sandwich", now, gerente, ingrprov.First().IngrPedProvs, GetPaymentMethods(0, 1).First());
            PedidoProv pedidoprov2 = new PedidoProv(2, precioTotal, "Calle Restaurante Sandwich", now, gerente, ingrprov.First().IngrPedProvs, GetPaymentMethods(1, 1).First());


            List<PedidoProv> allPedidos = new List<PedidoProv> { pedidoprov1, pedidoprov2 };

            return allPedidos.GetRange(id, numOfPurchases);
        }

        public static IList<MetodoDePago> GetPaymentMethods(int index, int numOfPayments)
        {
            List<MetodoDePago> allPaymentMethods = new List<MetodoDePago>
            {
                new Tarjeta { Id = 1, Numero = 1234567890123456, CCV = 123, MesCaducidad = 05, AnoCaducidad = 2023 },
                new Efectivo { Id= 2, NecesitasCambio = false },
            };
            return allPaymentMethods.GetRange(index, numOfPayments);
        }
    }
}
