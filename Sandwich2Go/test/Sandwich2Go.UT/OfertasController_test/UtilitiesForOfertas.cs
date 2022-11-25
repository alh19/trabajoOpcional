using Sandwich2Go.Data;
using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandwich2Go.UT.OfertasController_test
{
    public class UtilitiesForOfertas
    {
        public static void InitializeDbOfertasForTests(ApplicationDbContext db, IList<Oferta> ofertas)
        {
            db.Oferta.AddRange(ofertas);
            db.SaveChanges();
        }

        public static void ReInitializeDbOfertasForTests(ApplicationDbContext db)
        {
            db.OfertaSandwich.RemoveRange(db.OfertaSandwich);
            db.Oferta.RemoveRange(db.Oferta);
            db.SaveChanges();
        }

        public static IList<Oferta> GetOfertas(int index, int numOfPurchases,
            IList<Sandwich> sandwiches, Gerente gerente)
        {
            Oferta oferta1 = new Oferta(1,
                        "oferta1",
                        System.DateTime.Now, new DateTime(2022, 12, 12, 8, 30, 52),
                        "descripcion1",
                        new List<OfertaSandwich>()
                        {
                           new OfertaSandwich(){Porcentaje = 30,Sandwich = sandwiches[0], SandwichId = sandwiches[0].Id,OfertaId=1}
                        },
                        gerente);
            oferta1.OfertaSandwich[0].Oferta = oferta1;
            Oferta oferta2 = new Oferta(2,
                        "oferta2",
                        System.DateTime.Now, new DateTime(2022, 12, 30, 9, 40, 32),
                        "descripcion2",
                        new List<OfertaSandwich>()
                        {
                           new OfertaSandwich(){Porcentaje = 50,Sandwich = sandwiches[1], SandwichId = sandwiches[1].Id,OfertaId=2}
                        },
                        gerente);
            oferta2.OfertaSandwich[0].Oferta = oferta2;

            List<Oferta> allOfertas = new List<Oferta> { oferta1, oferta2, };

            return allOfertas.GetRange(index, numOfPurchases);
        }
    }
}
