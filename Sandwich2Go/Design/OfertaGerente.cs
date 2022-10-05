using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class OfertaGerente
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual Oferta Oferta { get; set; }
        public virtual Gerente Gerente { get; set; }
    }
}
