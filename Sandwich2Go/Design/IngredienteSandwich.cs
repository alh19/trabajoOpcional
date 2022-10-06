using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Design
{
    public class IngredienteSandwich
    {
        [Key]
        public virtual int Id { get; set; }
        [ForeignKey("IngredienteId")]
        public virtual Ingrediente Ingrediente
        {
            get;
            set;
        }
        public virtual int IngredienteId { get; set; }

        [ForeignKey("SandwichId")]
        public virtual Sandwich Sandwich
        {
            get;
            set;
        }
        public virtual int SandwichId { get; set; }

        [Required, Display(Name = "Cantidad ingredientes")]
        [Range(0, 4, ErrorMessage = "La cantidad máxima de un mismo ingrediente es 4")]
        public virtual int Cantidad { get; set; }
    }
}