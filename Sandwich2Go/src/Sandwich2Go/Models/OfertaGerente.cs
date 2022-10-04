using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class OfertaGerente
    {
        [Key]
        public int Id { get; set; }
        public Oferta Oferta
        {
            get => default;
            set
            {
            }
        }
        public Gerente Gerente
        {
            get => default;
            set
            {
            }
        }
    }
}
