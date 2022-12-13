using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class SandwCreado:Sandwich
    {
        public SandwCreado(string sandwichName, double precio, string desc, IList<SandwichPedido> sandwichPedido, IList<IngredienteSandwich> ingredienteSandwich, IList<OfertaSandwich> ofertaSandwich) : base(sandwichName, precio, desc, sandwichPedido, ingredienteSandwich, ofertaSandwich)
        {
        }

        [Required]
        public virtual int Cantidad { get; set; }

        [Required]
        public virtual IList<Pedido> Pedidos { get; set; }
    }
}
