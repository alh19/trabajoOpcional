using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Design
{
    public class ArticulosPed
    {
        [Key]
        public virtual int Id { get; set; }
        [Required]
        public virtual Ingrediente Ingrediente { get; set; }
        [Required]
        public virtual PedidoProv PedidoProv { get; set; }

    }
}
