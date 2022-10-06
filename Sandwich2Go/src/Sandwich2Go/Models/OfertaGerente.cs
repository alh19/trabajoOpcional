using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class OfertaGerente
    {
        [Key]
        public virtual int Id { get; set; }
        [Required]
        public virtual Oferta Oferta { get; set; }
        [Required]
        public virtual Gerente Gerente { get; set; }
    }
}
