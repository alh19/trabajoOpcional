using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.IngredienteViewModels
{
    public class IngrProvForPurchaseViewModel
    {
        public IngrProvForPurchaseViewModel()
        {

        }

        public IngrProvForPurchaseViewModel(Ingrediente ingrprov)
        {
            Id = ingrprov.Id;
            Nombre = ingrprov.Nombre;
            Stock = ingrprov.Stock;
        }

        [Key]
        public int Id { get; set; }


        [Required, StringLength(20, ErrorMessage = "First name cannot be longer than 20 characters.")]
        public string Nombre { get; set; }


        [Required, Display(Name = "stock")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public int Stock { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IngrProvForPurchaseViewModel model &&
                Id == model.Id &&
                Nombre == model.Nombre &&
                Stock == model.Stock;
        }
    }
}
