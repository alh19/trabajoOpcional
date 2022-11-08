using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.ProveedorViewModels
{
    public class SelectProveedoresForPurchaseViewModel
    {
        public IEnumerable<Proveedor> Proveedores { get; set; }
        //Utilizado para filtrar por nombres
        [Display(Name = "Nombre")]
        public string proveedorNombreSelected { get; set; }
    }
}
