using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class ArticulosPed
    {
        [Key]
        public virtual int Id { get; set; }

        [Required, Display(Name = "cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
        public virtual int Cantidad { get; set; }

        [ForeignKey("IngredienteId")]
        public virtual Ingrediente Ingrediente { get; set; }
        public virtual int IngredienteId { get; set; }

        [ForeignKey("PedidoId")]
        public virtual PedidoProv PedidoProv { get; set; }
        public virtual int PedidoId { get; set; }

    }
}
