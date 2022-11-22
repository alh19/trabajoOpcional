using System;
using System.Collections.Generic;
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
            foreach (IngredienteSandwich ingSand in sandwichPedido.IngredienteSandwich)
            {
                this.Ingredientes.Add(ingSand.Ingrediente.Nombre);
                foreach(AlergSandw alSand in ingSand.Ingrediente.AlergSandws)
                {
                    if (!this.Alergenos.Contains(alSand.Alergeno.Name))
                    {
                        this.Alergenos.Add(alSand.Alergeno.Name);
                    }
                }
            }

        }


        public virtual string NombreSandwich
        {
            get; set;
        }

        public virtual double PrecioCompra
        {
            get; set;
        }

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
                this.NombreSandwich == model.NombreSandwich &&
                this.PrecioCompra == model.PrecioCompra &&
                this.Ingredientes.SequenceEqual(model.Ingredientes) &&
                this.Alergenos.SequenceEqual(model.Alergenos);
        }

    }
}
