using System;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class MesaReserva
    {
        [Key]
        public virtual int ReservaId { get; set; }
        [Required]
        public virtual int IdMesa { get; set; }
        [Required]
        public virtual int ClienteId { get; set; }
        [Required]
        public virtual DateTime FechaReserva { get; set; }
        [Required]
        public virtual int NumPersonas { get; set; }
    }
}
