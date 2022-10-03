using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Cliente : IdentityUser
    {
        [Key]
        public override string Id { get; set; }
        [Required]
        public virtual string Nombre { get; set; }
        [Required]
        public virtual string Apellido { get; set; }
        [Required]
        [RegularExpression("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$")]
        public override string Email { get; set; }
        [Required]
        public override string PasswordHash { get; set; }
        [Required]
        public override string PhoneNumber { get; set; }
        [Required]
        public virtual string Direccion { get; set; }
        public virtual IList<MesaReserva> ReservaMesa { get; set; }
    }
}
