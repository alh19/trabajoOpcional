using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Mesa
    {
        [Key]
        public virtual int Id { get; set; }
        [Required]
        public virtual int Capacidad { get; set; }
        [Required]
        public virtual string Estado { get; set; }
        public virtual IList<MesaReserva> ReservaMesa { get; set; }
    }
}
