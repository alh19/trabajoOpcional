using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models.PedidoViewModels
{
    public class PedidoIndexViewModel
    {
        public virtual int Id { get; set; }
        public virtual string fechaCompra { get; set; }
        [DataType(DataType.Currency)]
        public virtual double precio { get; set; }
        public virtual string direccion { get; set; }

        public PedidoIndexViewModel(Pedido p)
        {
            Id = p.Id;
            fechaCompra = p.Fecha.ToString();
            precio = p.Preciototal;
            direccion = p.Direccion;
        }

        public override bool Equals(object obj)
        {
            return obj is PedidoIndexViewModel model &&
                this.Id == model.Id &&
                this.fechaCompra == model.fechaCompra &&
                this.precio == model.precio &&
                this.direccion == model.direccion;
        }
    }

}
