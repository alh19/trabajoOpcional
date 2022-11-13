using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.IngredienteViewModels
{
    public class SelectedIngrProvForPurchaseViewModel
    {

        public string[] IdsToAdd { get; set; }
        [Display(Name = "Nombre")]
        public string ingredienteNombre { get; set; }
        [Display(Name = "Stock")]
        public int ingredienteStock { get; set; }

        public int IdProveedor { get; set; }
    }
}
