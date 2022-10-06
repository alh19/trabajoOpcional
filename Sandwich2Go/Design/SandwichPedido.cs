using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class SandwichPedido
    {
        [Key]
        public virtual int Id { get; set; }
        [ForeignKey("SandwichId")]
        public virtual Sandwich Sandwich
        {
            get;
            set;
        }

        public virtual int SandwichId { get; set; }

        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido
        {
            get;
            set;
        }
        public virtual int PedidoId { get; set; }
        [Required, Display(Name = "Maximo sándwiches")]
        [Range(1, 10, ErrorMessage = "El máximo de un mismo sándwich por pedido es 10")]
        public virtual int Cantidad { get; set; }
    }
}
