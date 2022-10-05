using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class OfertaGerente
    {
        [Key]
        public int Id { get; set; }
        public Oferta Oferta { get;  set; }
        public Gerente Gerente { get; set; }
    }
}
