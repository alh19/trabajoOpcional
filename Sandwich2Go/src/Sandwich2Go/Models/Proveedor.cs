using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Proveedor
    {
        [Key]
        public virtual int Id { get; set; } 

        [Required, Display(Name = "CIF o DNI")]
        [StringLength(8, ErrorMessage = "El CIF no puede ser inferior a 8 dígitos")]
        public virtual string Cif { get; set; }

        [Required]
        public virtual string Nombre { get; set; }

        public virtual string Direccion { get; set; }


        public virtual IList<IngrProv> IngrProv { get; set; }
    }
}
