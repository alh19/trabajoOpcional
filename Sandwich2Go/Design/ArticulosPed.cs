using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Design
{
    public class ArticulosPed
    {
        [Key]
        public virtual int Id { get; set; }

        [ForeignKey("IdIngrediente")]
        public virtual Ingrediente Ingrediente { get; set; }
        public virtual string IdIngredientes { get; set; }

        [ForeignKey("IdPedidoProv")]
        public virtual PedidoProv PedidoProv { get; set; }
        public virtual int idPedidoProv { get; set; }


    }
}
