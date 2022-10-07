using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class OfertaGerente
    {
        [Key]
        public virtual string Id { get; set; }
        [ForeignKey("GerenteId")]
        public virtual Gerente Gerente { get; set; }
        public virtual string GerenteId { get; set; }

        [ForeignKey("OfertaId")]
        public virtual Oferta Oferta { get; set; }
        public virtual int OfertaId { get; set; }
    }
}
