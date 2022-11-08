using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SelectedProveedoresForPurchaseViewModel
    {

        public string[] IdsToAdd { get; set; }
        [Display(Name = "Nombre")]
        public string nombreProveedorSelected { get; set; }

        [Display(Name = "CIF")]
        public string proveedorCif { get; set; }

    }
}
