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
        public virtual int Id { get; set; }
        [Required]
        public virtual Ingrediente Ingrediente
        {
            get;
            set;
        }
        [Required]
        public virtual Sandwich Sandwich
        {
            get;
            set;
        }
        [Required, Display(Name = "Cantidad ingredientes")]
        [Range(0, 4, ErrorMessage = "La cantidad máxima de un mismo ingrediente es 4")]
        public virtual int Cantidad { get; set; }
    }
}