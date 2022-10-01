using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Cliente
    {
        [Key]
        public virtual int ClienteId { get; set; }
        [Required]
        public virtual string Nombre { get; set; }
        [Required]
        public virtual string Correo { get; set; }
        [Required]
        public virtual string Password { get; set; }
        [Required]
        public virtual int Telefono { get; set; }
        [Required]
        public virtual string Direccion { get; set; }
    }
}
