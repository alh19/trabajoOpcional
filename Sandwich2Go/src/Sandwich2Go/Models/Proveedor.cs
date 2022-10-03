using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Proveedor
    {
        [Key]
        public virtual string Id { get; set; }
        [Required]
        public virtual string Cif { get; set; }
        [Required]
        public virtual string Nombre { get; set; }
        public virtual string Direccion { get; set; }

    }
}
