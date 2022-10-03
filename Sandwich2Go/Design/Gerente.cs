using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Gerente : IdentityUser
    {
            [Required]
            public virtual string Nombre { get; set; }
            [Required]
            public virtual string Apellido { get; set; }
            [Required]
            public virtual string Direccion { get; set; }
    }
}
