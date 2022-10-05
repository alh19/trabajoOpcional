﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Design
{
    public class PedidoProv
    {

        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual int IdPedidoProv { get; set; }

        [ForeignKey("IdProveedor")]
        public virtual Proveedor Proveedor { get; set; }
        public virtual string IdProveedor { get; set; }

        public virtual IList<ArticulosPed> ArticulosPed { get; set; }
    }
}