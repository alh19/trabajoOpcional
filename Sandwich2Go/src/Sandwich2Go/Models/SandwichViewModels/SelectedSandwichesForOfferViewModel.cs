using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SelectedSandwichesForOfferViewModel
    {
        public string[] IdsToAdd { get; set; }
        [Display(Name = "Nombre")]
        public string sandwichNombre { get; set; }
        [Display(Name = "Precio")]
        public string sandwichPrecio { get; set; }
    }
}
