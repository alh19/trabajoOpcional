using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Cliente : IdentityUser
    {
        [Required]
        public virtual string ClienteId { get; set; }
        [Required]
        public virtual string Nombre { get; set; }
        [Required]
        public virtual string Apellido { get; set; }
        [Required]
        public virtual string Direccion { get; set; }
        public virtual IList<MesaReserva> ReservaMesa { get; set; }
    }
}
