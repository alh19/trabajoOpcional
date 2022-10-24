using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Design
{
    public class IngrPedProv
    {

        [Required, Display(Name = "cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
        public virtual int Cantidad { get; set; }
        public virtual int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public virtual PedidoProv PedidoProv { get; set; }
        [ForeignKey("IngrProvId")]
        public virtual IngrProv IngrProv { get; set; }

    }
}
