using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class OfertaSandwich
    {
        [Key]
        public virtual string Id { get; set; }
        public Oferta Oferta { get; set; }
        public Sandwich Sandwich { get; set; }
    }
}
