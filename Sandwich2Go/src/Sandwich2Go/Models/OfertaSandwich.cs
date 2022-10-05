using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class OfertaSandwich
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual Oferta Oferta { get; set; }
        public virtual Sandwich Sandwich { get; set; }
        [Required, Display(Name = "Máxima cantidad de un mismo sándwich")]
        [Range(1, 3, ErrorMessage = "El máximo de un mismo sándwich por oferta es 3")]
        public virtual int Cantidad { get; set; }
    }
}
