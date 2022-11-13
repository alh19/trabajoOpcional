using Sandwich2Go.Models.IngredienteViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SandwichForOfferViewModel
    {
        public SandwichForOfferViewModel()
        {

        }

        public SandwichForOfferViewModel(Sandwich sandwich)
        {
            Id = sandwich.Id;
            SandwichName = sandwich.SandwichName;
            Precio = sandwich.Precio;
            Desc = sandwich.Desc;
        }

        [Key]
        public int Id { get; set; }

        [Required, StringLength(20, ErrorMessage = "El nombre no puede ser mayor a 20 caracteres.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string SandwichName { get; set; }

        [Required, DataType(DataType.Currency)]
        public double Precio { get; set; }
        [Required, StringLength(100, ErrorMessage = "La descripción no puede ser mayor a 100 caracteres.")]
        public string Desc { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SandwichForOfferViewModel model &&
                Id == model.Id &&
                SandwichName == model.SandwichName &&
                Precio == model.Precio &&
                Desc == model.Desc;
        }
    }
}
