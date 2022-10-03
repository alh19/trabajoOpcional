using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Ingrediente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdIngrediente { get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public int cantidad { get; set; }

        [Required]
        public int stock { get; set; }

    }
}
