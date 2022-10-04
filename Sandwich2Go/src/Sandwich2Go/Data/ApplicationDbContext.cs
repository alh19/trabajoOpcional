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
        public DbSet <Sandwich> Sandwich { set; get; }
        public DbSet<Alergeno> Alergeno { set; get; }
        public DbSet<SandwichPedido> SandwichPedido { set; get; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Mesa> Mesa { get; set; }
        public DbSet<MesaReserva> MesaReserva { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Ingrediente> Ingrediente { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        //public DbSet<Gerente> Gerente { get; set; }
        //public DbSet<PedidoProv> PedidoProv { get; set; }
        //public DbSet<ArticulosPed> ArticulosPed { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MesaReserva>()
            .HasAlternateKey(pi => new { pi.ClienteId, pi.MesaId });
            builder.Entity<Ingrediente>()
            .HasAlternateKey(i => new { i.nombre });
        }


    }
}
