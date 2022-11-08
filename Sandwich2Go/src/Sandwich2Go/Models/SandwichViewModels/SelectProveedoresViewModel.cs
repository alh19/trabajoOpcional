using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SelectProveedoresViewModel
    {

        public IEnumerable<Proveedor> Proveedores { get; set; }
        public SelectList Nombres;
        [Display(Name = "Nombre")]
        public string nombreProveedorSelected { get; set; }

        [Display(Name = "CIF")]
        public float proveedorCif { get; set; }
    }
}
