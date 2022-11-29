using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Usuario : IdentityUser
    {
        [Required, StringLength(20, ErrorMessage = "El nombre no puede contener más de 20 caracteres")]
        public virtual string Nombre { get; set; }
        [Required, StringLength(20, ErrorMessage = "El apellido no puede contener más de 20 caracteres")]
        public virtual string Apellido { get; set; }
        [Required, StringLength(80, ErrorMessage = "La dirección no puede contener más de 80 caracteres")]
        public virtual string Direccion { get; set; }
    }
}
