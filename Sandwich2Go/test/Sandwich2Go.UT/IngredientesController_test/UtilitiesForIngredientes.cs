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

namespace Sandwich2Go.UT.IngredientesController_test
{
    public static class UtilitiesForIngredientes
    {
        public static void InitializeDbAlergenosForTests(ApplicationDbContext db)
        {
            db.Alergeno.AddRange(GetAlergenos(0, 4));
            db.SaveChanges();

        }

        public static void ReInitializeDbAlergenosForTests(ApplicationDbContext db)
        {
            db.Alergeno.RemoveRange(db.Alergeno);
            db.SaveChanges();
        }
        




        

        public static IList<Ingrediente> GetIngredientes(int index, int numOfIngredientes)
        {
            IList<Alergeno> alergenos = GetAlergenos(0, 2);
            
           
            var allIngredientes = new List<Ingrediente>
            {
                new Ingrediente {Id=1,Nombre="Lechuga",PrecioUnitario=2,Stock=8,AlergSandws = new List<AlergSandw>{ new AlergSandw {Id=1,IngredienteId=1,Alergeno = alergenos[0]}}},
                new Ingrediente {Id=2,Nombre="Tomate",PrecioUnitario=3,Stock=9,AlergSandws = new List<AlergSandw>{ new AlergSandw {Id=2,IngredienteId=2,Alergeno = alergenos[1]}}}
            };

            return allIngredientes.GetRange(index, numOfIngredientes);
        }

        public static IList<Alergeno> GetAlergenos(int index, int numOfAlergenos)
        {
            var allAlergenos = new List<Alergeno>
                {
                    
                    new Alergeno { id=1, Name="Lactosa" } ,
                    new Alergeno { id=2, Name="Gluten" } ,
                    new Alergeno { id=3, Name="Soja" } ,
                    new Alergeno { id=4, Name="Huevo" } 
                };
            //return from the list as much instances as specified in numOfAlergenos
            return allAlergenos.GetRange(index, numOfAlergenos);
        }

        public static void InitializeDbIngredientesForTests(ApplicationDbContext db)
        {

            db.Ingrediente.AddRange(GetIngredientes(0, 2));
            
            db.Alergeno.AddRange(GetAlergenos(2, 2));
            db.SaveChanges();

            db.Users.Add(new Cliente { Id = "3", UserName = "peter@uclm.com", Email = "peter@uclm.com", Nombre = "Peter", Apellido = "Jackson Jackson", EmailConfirmed = true, Direccion = "" });
            db.SaveChanges();
        }

        public static void ReInitializeDbIngredientesForTests(ApplicationDbContext db)
        {
            db.Ingrediente.RemoveRange(db.Ingrediente);
            db.Alergeno.RemoveRange(db.Alergeno);
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
        }





    }
}

