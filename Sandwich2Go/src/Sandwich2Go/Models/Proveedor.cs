using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Proveedor
    {
        [Key]
        public virtual string cif { get; set; }
        [Required]
        public virtual string nombre { get; set; }
        [Required]
        public virtual int tlf { get; set; }
        [Required]
        [RegularExpression("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")]
        public virtual string email { get; set; } 
        public virtual string direccion { get; set; }

    }
}
