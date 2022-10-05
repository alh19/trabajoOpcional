using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class PedidoProv
    {

        [Key]
        public virtual int Id { get; set; }

        public Proveedor Proveedor { get; set; }

        public virtual IList<ArticulosPed> ArticulosPed { get; set; }
    }
}
