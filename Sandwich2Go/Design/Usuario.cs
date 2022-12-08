using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Usuario : IdentityUser
    {
        [Required, StringLength(20, ErrorMessage = "El nombre no puede contener más de 20 caracteres")]
        public virtual string Nombre { get; set; }
        [Required, StringLength(20, ErrorMessage = "El apellido no puede contener más de 20 caracteres")]
        public virtual string Apellido { get; set; }
        [Required, StringLength(80, ErrorMessage = "La dirección no puede contener más de 80 caracteres")]
        public virtual string Direccion { get; set; }
        public override bool Equals(object obj)
        {
            return obj is Usuario user &&
                   Id == user.Id &&
                   Email == user.Email &&
                   PhoneNumber == user.PhoneNumber &&
                   EqualityComparer<DateTimeOffset?>.Default.Equals(LockoutEnd, user.LockoutEnd) &&
                   LockoutEnabled == user.LockoutEnabled &&
                   Nombre == user.Nombre &&
                   Apellido == user.Apellido &&
                   Direccion == user.Direccion;
        }
    }
}
