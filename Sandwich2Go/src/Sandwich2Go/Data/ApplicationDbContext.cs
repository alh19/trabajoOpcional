using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sandwich2Go.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
       
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    }
}
