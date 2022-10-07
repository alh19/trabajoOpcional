﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class PedidoProv
    {

        [Key]
        public virtual int Id { get; set; }
        [Required]
        public virtual Proveedor Proveedor { get; set; }
        [Required]
        public virtual Gerente Gerente { get; set; }
        public virtual IList<ArticulosPed> ArticulosPed { get; set; }
    }
}
