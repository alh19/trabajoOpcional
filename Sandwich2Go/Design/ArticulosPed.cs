using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Design
{
    public class ArticulosPed
    {
        [Key]
        public virtual int Id { get; set; }

        [ForeignKey("IngredienteId")]
        public virtual Ingrediente Ingrediente { get; set; }
        public virtual int IngredienteId { get; set; }

        [ForeignKey("PedidoId")]
        public virtual PedidoProv PedidoProv { get; set; }
        public virtual int PedidoId { get; set; }

    }
}
