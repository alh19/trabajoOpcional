using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.IngredienteViewModels
{ 
    public class SelectIngrProvForPurchaseViewModel
    {
        public IEnumerable<Ingrediente> Ingredientes { get; set; }
        //Utilizado para filtrar por nombres
        [Display(Name = "Nombre")]
        public string ingredienteNombre { get; set; }
        [Display(Name = "Stock")]
        public string ingredienteStock { get; set; }
        public int IdProveedor { get; set; }
    }
}
