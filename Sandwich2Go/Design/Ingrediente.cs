using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Ingrediente
    {
        
        [Key]
        public virtual int Id { get; set; }

        
        [Required, StringLength(20, ErrorMessage = "First name cannot be longer than 20 characters.")]
        public virtual string Nombre { get; set; }

        
        [Required, Display(Name = "cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public virtual int Cantidad { get; set; }

        
        [Required, Display(Name = "stock")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public virtual int Stock { get; set; }
        public virtual IList<AlergSandw> AlergSandws { get; set; }

        public virtual IList<IngredienteSandwich> IngredienteSandwich
        {
            get;
            set;
        }

        public virtual IList<ArticulosPed> ArticulosPeds { get; set; }
    }
}
