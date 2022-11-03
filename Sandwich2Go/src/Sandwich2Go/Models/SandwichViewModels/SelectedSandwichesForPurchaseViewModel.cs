using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SelectedSandwichesForPurchaseViewModel
    {
        public string[] IdsToAdd { get; set; }
        [Display(Name = "Alergeno")]
        public string sandwichAlergenoSelected { get; set; }

        [Display(Name = "Precio")]
        public string sandwichPrecio { get; set; }
    }
}
