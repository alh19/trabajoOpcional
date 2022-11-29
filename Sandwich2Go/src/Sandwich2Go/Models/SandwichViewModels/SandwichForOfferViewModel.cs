using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SandwichForOfferViewModel
    {

        public SandwichForOfferViewModel(Sandwich sandwich)
        {
            Id = sandwich.Id;
            SandwichName = sandwich.SandwichName;
            Precio = sandwich.Precio;
            Descripcion = sandwich.Desc;
            ingredientes = new string[sandwich.IngredienteSandwich.Count];
            int i = 0;
            foreach (IngredienteSandwich ing in sandwich.IngredienteSandwich)
            {
                ingredientes[i] = ing.Ingrediente.Nombre + " ";
                i++;
            }
        }

        [Key]
        public int Id { get; set; }

        [Required, StringLength(20, ErrorMessage = "El nombre no puede ser mayor a 20 caracteres.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string SandwichName { get; set; }

        [Required, DataType(DataType.Currency)]
        public double Precio { get; set; }

        [Required]
        public string[] ingredientes { get; set; }

        public string Descripcion { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SandwichForOfferViewModel model &&
                Id == model.Id &&
                SandwichName == model.SandwichName &&
                Precio == model.Precio &&
                Descripcion == model.Descripcion &&
                ingredientes.SequenceEqual(model.ingredientes);
        }
    }
}
