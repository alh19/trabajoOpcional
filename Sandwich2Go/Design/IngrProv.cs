using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Design
{
    internal class IngrProv
    {

        [Key]
        public virtual int Id { get; set; }

        [Required, Display(Name = "cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
        public virtual int Cantidad { get; set; }

        [ForeignKey("IngredienteId")]
        public virtual Ingrediente Ingrediente { get; set; }

        [Required]
        public virtual Proveedor Proveedor { get; set; }

    }
}
