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
        public Sandwich Sandwich
        {
            get => default;
            set
            {
            }
        }

        public Pedido Pedido
        {
            get => default;
            set
            {
            }
        }
    }
}
