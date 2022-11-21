using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandwich2Go.Models.PedidoViewModels
{
    public class PedidoSandwichViewModel
    {
        public PedidoSandwichViewModel() { }

        public PedidoSandwichViewModel (SandwichPedido sandwichPedido)
        {
            this.NombreSandwich = sandwichPedido.Sandwich.SandwichName;
            this.PrecioCompra = sandwichPedido.Sandwich.Precio;
            this.Ingredientes = new List<string>();
            this.Alergenos = new List<string>();
            foreach (IngredienteSandwich ingSand in sandwichPedido.Sandwich.IngredienteSandwich)
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
    }
}
