using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class SandwCreado:Sandwich
    {
        [Required]
        public virtual int Cantidad { get; set; }
    }
}
