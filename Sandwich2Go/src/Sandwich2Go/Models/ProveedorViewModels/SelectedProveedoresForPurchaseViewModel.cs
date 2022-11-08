using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.ProveedorViewModels
{
    public class SelectedProveedoresForPurchaseViewModel
    {

        public string[] IdsToAdd { get; set; }

        //Utilizado para filtrar por nombre (listado)
        public SelectList Nombres;
        [Display(Name = "Nombre")]
        public string nombreProveedorSelected { get; set; }

        //Utilizado para filtrar por CIF (string)
        [Display(Name = "CIF")]
        public string proveedorCif { get; set; }

    }
}
