using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Sandwich2Go.Models
{
    public class Ingrediente
    {
        
        [Key]
        public virtual int Id { get; set; }

        
        [Required, StringLength(20, ErrorMessage = "First name cannot be longer than 20 characters.")]
        public virtual string nombre { get; set; }

        
        [Required, Display(Name = "cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public virtual int cantidad { get; set; }

        
        [Required, Display(Name = "stock")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public virtual int stock { get; set; }

        public IList<IngredienteSandwich> IngredienteSandwich
        {
            get;
            set;
        }
    }
}
