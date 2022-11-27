using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sandwich2Go.Models.PedidoViewModels
{
    public class SandwichPedidoViewModel
    {
        public SandwichPedidoViewModel() { }

        public SandwichPedidoViewModel (Sandwich sandwichPedido)
        {
            this.Id = sandwichPedido.Id;
            this.NombreSandwich = sandwichPedido.SandwichName;
            this.PrecioCompra = sandwichPedido.Precio;
            this.Ingredientes = new List<string>();
            this.Alergenos = new List<string>();
            this.porcentajeOferta = 0;
            this.cantidad = 1;
            foreach (IngredienteSandwich ingSand in sandwichPedido.IngredienteSandwich)
            {
                this.Ingredientes.Add(ingSand.Ingrediente.Nombre+" ");
                foreach(AlergSandw alSand in ingSand.Ingrediente.AlergSandws)
                {
                    if (!this.Alergenos.Contains(alSand.Alergeno.Name+" "))
                    {
                        this.Alergenos.Add(alSand.Alergeno.Name + " ");
                    }
                }
            }
            if (sandwichPedido.OfertaSandwich != null) 
            {
                foreach (OfertaSandwich os in sandwichPedido.OfertaSandwich)
                {
                    if(os.Porcentaje>this.porcentajeOferta && os.Oferta.FechaFin>DateTime.Now)
                    {
                        this.oferta = os.Oferta.Nombre;
                        this.porcentajeOferta = os.Porcentaje;
                    }
                    this.descuento = this.PrecioCompra * (this.porcentajeOferta/100);
                    this.oferta = this.NombreSandwich +" con oferta "+this.oferta + " y descuento del " + this.porcentajeOferta + "% ..... -";
                    this.PrecioConDescuento = this.PrecioCompra - this.descuento;
                }
            }

        }
        [DataType(DataType.Currency)]
        public virtual double descuento { get; set; }
        public virtual int Id { get; set; }

        public virtual string oferta { get; set; }
        public virtual double porcentajeOferta { get; set; }
        public virtual string NombreSandwich
        {
            get; set;
        }
        [DataType(DataType.Currency)]
        public virtual double PrecioCompra
        {
            get; set;
        }
        [DataType(DataType.Currency)]
        public virtual double PrecioConDescuento
        {
            get; set;
        }

        public virtual int cantidad { get; set; }
        public virtual IList<string> Ingredientes
        {
            get; set;
        }

        public virtual IList<string> Alergenos
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            return obj is SandwichPedidoViewModel model &&
                this.Id == model.Id &&
                this.NombreSandwich == model.NombreSandwich &&
                this.PrecioCompra == model.PrecioCompra &&
                this.Ingredientes.SequenceEqual(model.Ingredientes) &&
                this.Alergenos.SequenceEqual(model.Alergenos) &&
                this.porcentajeOferta == model.porcentajeOferta &&
                this.oferta == model.oferta &&
                this.PrecioConDescuento == model.PrecioConDescuento &&
                this.descuento == model.descuento;
        }

    }
}
