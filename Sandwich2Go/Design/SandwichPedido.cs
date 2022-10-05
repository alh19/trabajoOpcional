using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class SandwichPedido
    {
        [Key]
        public int Id { get; set; }
        public Sandwich Sandwich
        {
            get;
            set;
        }

        public Pedido Pedido
        {
            get;
            set;
        }
        [Required, Display(Name = "Maximo sándwiches")]
        [Range(1, 10, ErrorMessage = "El máximo de un mismo sándwich por pedido es 10")]
        public int cantidad { get; set; }
    }
}
