using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Design
{
    public class Sandwich
    {
        [Key]
        public virtual string id { get; set; }
        [Required, StringLength(20, ErrorMessage = "El nombre no puede ser mayor a 20 caracteres.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public virtual string sandwichName { get; set;}
        [Required, DataType(DataType.Currency)]//Tipo moneda
        public virtual double precio { get; set; }
        [Required,StringLength(100, ErrorMessage = "La descripción no puede ser mayor a 100 caracteres.")]
        public virtual string desc { get; set; }

        public IList<SandwichPedido> sandwichPedido
        {
            get;
            set;
            
            
        }

        public IList<IngredienteSandwich> IngredienteSandwich
        {
            get => default;
            set
            {
            }
        }
    }
}
