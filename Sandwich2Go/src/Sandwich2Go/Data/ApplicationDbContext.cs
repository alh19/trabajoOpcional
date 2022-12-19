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
        public DbSet<Sandwich> Sandwich { set; get; }
        public DbSet<Alergeno> Alergeno { set; get; }
        public DbSet<SandwichPedido> SandwichPedido { set; get; }
        public DbSet<IngredienteSandwich> IngredienteSandwich { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Ingrediente> Ingrediente { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Oferta> Oferta { get; set; }
        public DbSet<PedidoProv> PedidoProv { get; set; }
        public DbSet<AlergSandw> AlergSandws { get; set; }
        public DbSet<IngrProv> IngrProv { get; set; }
        public DbSet<OfertaSandwich> OfertaSandwich { get; set; }
        public DbSet<MetodoDePago> MetodoDePago { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ingrediente>()
            .HasAlternateKey(i => new { i.Nombre });

            builder.Entity<Sandwich>()
            .HasAlternateKey(i => new { i.SandwichName });

            builder.Entity<Proveedor>()
            .HasAlternateKey(i => new { i.Cif });

            builder.Entity<AlergSandw>()
                .HasKey(a => new { a.IngredienteId, a.AlergenoId });

            builder.Entity<IngredienteSandwich>()
                .HasKey(i => new { i.IngredienteId, i.SandwichId });

            builder.Entity<OfertaSandwich>()
                .HasKey(o => new { o.OfertaId, o.SandwichId });

            builder.Entity<SandwichPedido>()
                .HasKey(p => new { p.SandwichId, p.PedidoId });

            builder.Entity<Sandwich>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Sandwich>("Sandwich")
                .HasValue<SandwCreado>("SandwCreado");

            builder.Entity<MetodoDePago>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<MetodoDePago>("MetodoDePago")
                .HasValue<Tarjeta>("Tarjeta")
                .HasValue<Efectivo>("Efectivo");

        }


    }
}
