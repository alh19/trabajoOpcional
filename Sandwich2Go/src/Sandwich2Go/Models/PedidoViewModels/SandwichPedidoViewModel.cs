﻿using System;
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

            this.NombreSandwich = sandwichPedido.SandwichName;
            this.PrecioCompra = sandwichPedido.Precio;
            this.Ingredientes = new List<string>();
            this.Alergenos = new List<string>();
            this.porcentajeOferta = 0;
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
                foreach (OfertaSandwich o in sandwichPedido.OfertaSandwich)
                {
                    if(o.Porcentaje>this.porcentajeOferta)
                    {
                        this.oferta = o.Oferta.Nombre;
                        this.porcentajeOferta = o.Porcentaje;
                    }
                    this.oferta = this.oferta + "con descuento del " + this.porcentajeOferta + "%";
                }
            }

        }

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

        public virtual int cantidad { get; set; }
        public virtual List<string> Ingredientes
        {
            get; set;
        }

        public virtual List<string> Alergenos
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
                this.Alergenos.SequenceEqual(model.Alergenos);
        }

    }
}
