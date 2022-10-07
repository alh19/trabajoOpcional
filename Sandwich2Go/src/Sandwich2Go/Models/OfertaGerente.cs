using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class OfertaGerente
    {
        [Key]
        public virtual int Id { get; set; }
        [ForeignKey("GerenteId")]
        public virtual Gerente Gerente { get; set; }
        public virtual int GerenteId { get; set; }

        [ForeignKey("OfertaId")]
        public virtual Oferta Oferta { get; set; }
        public virtual int OfertaId { get; set; }
    }
}
