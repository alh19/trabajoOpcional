﻿using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Proveedor
    {
        [Key]
        public virtual string Id { get; set; }

        [Required]
        public virtual string IdProveedor { get; set; }

        [Required, StringLength(8, ErrorMessage = "El CIF no puede ser inferior a 8 dígitos")]
        public virtual string Cif { get; set; }

        [Required]
        public virtual string Nombre { get; set; }

        public virtual string Direccion { get; set; }

        public virtual IList<PedidoProv> PedidoProv { get; set; }
    }
}
