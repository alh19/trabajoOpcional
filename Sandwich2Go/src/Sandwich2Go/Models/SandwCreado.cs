using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class SandwCreado:Sandwich
    {
        [Required]
        public virtual int Cantidad { get; set; }

        [Required]
        public virtual IList<Pedido> Pedidos { get; set; }
    }
}
