using Design;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.models;
using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sandwich2Go.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet <Sandwich> Sandwich { set; get; }
        public DbSet<Alergeno> Alergeno { set; get; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Mesa> Mesa { get; set; }
        public DbSet<MesaReserva> MesaReserva { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MesaReserva>()
            .HasAlternateKey(pi => new { pi.Id, pi.MesaId });
        }

    }
}
