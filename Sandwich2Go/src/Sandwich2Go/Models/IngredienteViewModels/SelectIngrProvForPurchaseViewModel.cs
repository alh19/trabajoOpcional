using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace Sandwich2Go.Models.IngredienteViewModels
{ 
    public class SelectIngrProvForPurchaseViewModel
    {
        public IEnumerable<IngredienteForPurchaseViewModel> Ingredientes { get; set; }
        //Utilizado para filtrar por nombres
        [Display(Name = "Nombre")]
        public string ingredienteNombre { get; set; }
        [Display(Name = "Stock")]
        public int ingredienteStock { get; set; }
        public int IdProveedor { get; set; }

        
        public override bool Equals(object obj)
        {
            return obj is IngredienteForPurchaseViewModel model &&
                ingredienteNombre == model.Nombre &&
                ingredienteStock == model.Stock;
        }
    }
}
