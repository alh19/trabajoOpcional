using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Cliente : Usuario
    {
        [Required]
        [CreditCard]
        public virtual string TarjetaCredito { get; set; }
        [DataType(DataType.Date), Display(Name = "Fecha de la última compra")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaUltimaCompra { get; set; }
        [DataType(DataType.Date), Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaNacimiento { get; set; }

        public virtual IList<Pedido> Pedidos { get; set; }
    }
}
