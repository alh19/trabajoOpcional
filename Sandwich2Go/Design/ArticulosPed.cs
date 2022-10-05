using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class ArticulosPed
    {
        [Key]
        public virtual int Id { get; set; }

        public Ingrediente Ingrediente { get; set; }

        public PedidoProv PedidoProv { get; set; }

    }
}
