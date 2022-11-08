using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.ProveedorViewModels
{
    public class SelectedProveedoresForPurchaseViewModel
    {

        public string[] IdsToAdd { get; set; }
        [Display(Name = "Nombre")]
        public string proveedorNombreSelected { get; set; }

    }
}
