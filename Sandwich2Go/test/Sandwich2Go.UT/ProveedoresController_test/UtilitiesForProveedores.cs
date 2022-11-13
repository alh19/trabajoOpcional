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

        public static void InitializeDbProveedoresForTests(ApplicationDbContext db)
        {
            db.Proveedor.Add(GetProveedor());
            db.SaveChanges();

            db.Users.Add(new Cliente { Id = "3", UserName = "peter@uclm.com", Email = "peter@uclm.com", Nombre = "Peter", Apellido = "Jackson Jackson", EmailConfirmed = true, Direccion = "" });
            db.SaveChanges();
        }

        public static Proveedor GetProveedor()
        {
            return new Proveedor { Id = 1, Nombre = "Alberto", Cif = "11111a", Direccion = "Calle1" };
        }

        public static void ReInitializeDbProveedoresForTests(ApplicationDbContext db)
        {
            db.IngrProv.RemoveRange(db.IngrProv);
            db.Proveedor.RemoveRange(db.Proveedor);

            db.SaveChanges();
        }
    }

}