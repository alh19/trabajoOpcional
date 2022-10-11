using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class SandwCreado:Sandwich
    {
        [Required]
        public virtual int Cantidad { get; set; }
    }
}
