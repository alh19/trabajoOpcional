﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class PedidoProv
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual string Nombre { get; set; }

        [Required]
        public virtual string Descripcion { get; set; }

        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha pedido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaPedido { get; set; }

        [Required]
        public virtual Proveedor Proveedor { get; set; }

        [Required]
        public virtual Gerente Gerente { get; set; }
        public virtual IList<ArticulosPed> ArticulosPed { get; set; }
    }
}
