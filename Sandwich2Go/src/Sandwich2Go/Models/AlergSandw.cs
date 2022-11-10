using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class AlergSandw
    { 

        [Key]  
        public virtual int Id { get; set; }

        [ForeignKey("AlergenoId")]
        public virtual Alergeno Alergeno { get; set; }
        public virtual int AlergenoId { get; set; }

        [ForeignKey("IngredienteId")]
        public virtual Ingrediente Ingrediente { get; set; }
        public virtual int IngredienteId { get; set; }

    }
}
