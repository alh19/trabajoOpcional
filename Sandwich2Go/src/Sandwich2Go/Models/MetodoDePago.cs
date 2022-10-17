using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sandwich2Go.Models
{

    public abstract class MetodoDePago
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public virtual IList<Pedido> Pedidos { get; set; }

        [Required]
        public virtual IList<PedidoProv> PedidoProvs { get; set; }
    }
}