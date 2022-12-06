using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Design
{
    public class Sandwich
    {
        public Sandwich(string sandwichName, double precio, string desc, IList<SandwichPedido> sandwichPedido, IList<IngredienteSandwich> ingredienteSandwich, IList<OfertaSandwich> ofertaSandwich)
        {
            SandwichName = sandwichName;
            Precio = precio;
            Desc = desc;
            SandwichPedido = sandwichPedido;
            IngredienteSandwich = ingredienteSandwich;
            OfertaSandwich = ofertaSandwich;
        }

        [Key]
        public virtual int Id { get; set; }
        [Required,StringLength(20, ErrorMessage = "El nombre no puede ser mayor a 20 caracteres.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public virtual string SandwichName { get; set;}
        [Required, DataType(DataType.Currency)]//Tipo moneda
        public virtual double Precio { get; set; }
        [Required,StringLength(100, ErrorMessage = "La descripción no puede ser mayor a 100 caracteres.")]
        public virtual string Desc { get; set; }

        public virtual IList<SandwichPedido> SandwichPedido
        {
            get;
            set;
            
            
        }

        public virtual IList<IngredienteSandwich> IngredienteSandwich
        {
            get;
            set;
        }
        public virtual IList<OfertaSandwich> OfertaSandwich { get; set; }
    }
}
