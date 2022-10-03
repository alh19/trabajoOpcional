using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sandwich2Go.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Mesa> Mesa { get; set; }
        public DbSet<MesaReserva> MesaReserva { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Ingrediente> Ingrediente { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MesaReserva>()
            .HasAlternateKey(pi => new { pi.Id, pi.MesaId });
            builder.Entity<Ingrediente>()
            .HasAlternateKey(i => new { i.nombre });
        }


    }
}
