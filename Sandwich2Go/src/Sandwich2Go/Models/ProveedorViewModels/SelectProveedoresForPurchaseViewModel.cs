using Microsoft.AspNetCore.Mvc.Rendering;
using Sandwich2Go.Models.IngredienteViewModels;
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

        /*public override bool Equals(object obj)
        {
            return obj is SelectProveedoresForPurchaseViewModel model &&
                Proveedores.SequenceEqual(model.Proveedores) &&
                proveedorNombreSelected == model.proveedorNombreSelected;
        }*/
    }
}
