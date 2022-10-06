using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Design
{
    public class OfertaSandwich
    {
        [Key]
        public virtual int Id { get; set; }
        [ForeignKey("OfertaId")]
        public virtual Oferta Oferta { get; set; }
        public virtual int OfertaId { get; set; }

        [ForeignKey("SandwichId")]
        public virtual Sandwich Sandwich { get; set; }
        public virtual int SandwichId { get; set; }
        [Required, Display(Name = "Máxima cantidad de un mismo sándwich")]
        [Range(1, 3, ErrorMessage = "El máximo de un mismo sándwich por oferta es 3")]
        public virtual int Cantidad { get; set; }
    }
}
