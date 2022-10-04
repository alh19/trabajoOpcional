using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Mesa
    {
        [Key]
        public virtual int Id { get; set; }
        [Required]
        [Range(2, 16, ErrorMessage = "La capacidad de la mesa tiene que ser como mínimo para 2 personas y como máximo para 16")]
        public virtual int Capacidad { get; set; }
        [Required]
        public virtual string Estado { get; set; }
        public virtual IList<MesaReserva> ReservaMesa { get; set; }
    }
}
