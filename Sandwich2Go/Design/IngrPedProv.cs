using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Design
{
    public class IngrPedProv
    {
        public IngrPedProv(int Id, int Cantidad, PedidoProv pedidoProv, int PedidoProvId,
            IngrProv ingrProv, int IngrProvId)
        {
            this.Id = Id;
            this.Cantidad = Cantidad;
            this.PedidoProv = PedidoProv;
            this.PedidoProvId = PedidoProvId;
            this.IngrProv = IngrProv;
            this.IngrProvId = IngrProvId;
        }

        public IngrPedProv()
        {

        }

        [Key]
        public virtual int Id { get; set; }
        [Required, Display(Name = "cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
        public virtual int Cantidad { get; set; }
        //public virtual int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public virtual PedidoProv PedidoProv { get; set; }
        public virtual int PedidoProvId { get; set; }
        [ForeignKey("IngrProvId")]
        public virtual IngrProv IngrProv { get; set; }
        public virtual int IngrProvId { get; set; }

    }
}
