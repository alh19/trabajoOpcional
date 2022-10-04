using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Design
{
    public class IngredienteSandwich
    {
        [Key]
        public int Id { get; set; }
        public Ingrediente Ingrediente
        {
            get;
            set;
        }

        public Sandwich Sandwich
        {
            get;
            set;
        }
        [Required, Display(Name = "Cantidad ingredientes")]
        [Range(0, 4, ErrorMessage = "La cantidad máxima de un mismo ingrediente es 4")]
        public int cantidad { get; set; }
    }
}