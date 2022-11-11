using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Controllers;
using Sandwich2Go.Data;
using Sandwich2Go.Models;
using Sandwich2Go.Models.ProveedorViewModels;
using Sandwich2Go.UT.ProveedoresController_test;
using Sandwich2Go.UT.SandwichControllers_test;
using Xunit;

namespace Sandwich2Go.UT.ProveedoresController_test
{
    public class SelectProveedoresForPurchase_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;

        public SelectProveedoresForPurchase_test()
        {
            _contextOptions = UtilitiesForProveedores.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            UtilitiesForProveedores.InitializeDbProveedoresForTests(context);
        }
    }
}
