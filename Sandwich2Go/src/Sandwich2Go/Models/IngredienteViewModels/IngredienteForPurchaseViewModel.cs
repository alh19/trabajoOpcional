using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.IngredienteViewModels
{
    public class IngredienteForPurchaseViewModel
    {
        public IngredienteForPurchaseViewModel()
        {

        }

        public IngredienteForPurchaseViewModel(Ingrediente ingrediente)
        {
            Id = ingrediente.Id;
            Nombre = ingrediente.Nombre;
            PrecioUnitario = ingrediente.PrecioUnitario;
            Stock = ingrediente.Stock;
        }

        [Key]
        public int Id { get; set; }


        [Required, StringLength(20, ErrorMessage = "First name cannot be longer than 20 characters.")]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio unitario")]
        public int PrecioUnitario
        {
            get;
            set;
        }

        [Required, Display(Name = "stock")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public int Stock { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IngredienteForPurchaseViewModel model &&
                Id == model.Id &&
                Nombre == model.Nombre &&
                PrecioUnitario == model.PrecioUnitario &&
                Stock == model.Stock;
        }
    }
}
