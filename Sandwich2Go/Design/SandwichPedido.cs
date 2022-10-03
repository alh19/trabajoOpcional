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
        [ForeignKey("idPedido")]
        public virtual Pedido Pedido { get; set; }
        public virtual int idPedido { get; set; }
        [ForeignKey("idSandwich")]
        public virtual Sandwich Sandwich { get; set; }

        public virtual int idSandwich { get; set; }

    }
}
