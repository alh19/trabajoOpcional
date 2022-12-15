using Sandwich2Go.Models.PedidoSandwichPersonalizadoViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class SandwCreado:Sandwich
    {
        public SandwCreado()
        {
        }

        public SandwCreado(string sandwichName, double precio, string desc, IList<SandwichPedido> sandwichPedido, IList<IngredienteSandwich> ingredienteSandwich, IList<OfertaSandwich> ofertaSandwich) : base(sandwichName, precio, desc, sandwichPedido, ingredienteSandwich,ofertaSandwich)
        {
        }

        
        public virtual int Cantidad { get; set; }

      
    }
}
