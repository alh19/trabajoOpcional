using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class AlergSandw
    {
        [Key]  
        public virtual int Id { get; set; }

        [Required]
        public virtual Alergeno Alergeno { get; set; }

        [Required]
        public virtual Ingrediente Ingrediente { get; set; }

    }
}
