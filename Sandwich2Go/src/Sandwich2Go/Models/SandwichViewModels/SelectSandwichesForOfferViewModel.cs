using Microsoft.AspNetCore.Mvc.Rendering;
using Sandwich2Go.Models.IngredienteViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SelectSandwichesForOfferViewModel
    {
        public IEnumerable<SandwichForOfferViewModel> Sandwiches { get; set; }
        //Utilizado para filtrar por nombre del Sándwich
        [Display(Name = "Nombre")]
        public string SandwichName { get; set; }
        //Utilizado para filtrar por precio del Sándwich
        [Display(Name = "Precio")]
        public double sandwichPrecio { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SelectSandwichesForOfferViewModel model &&
                SandwichName == model.SandwichName &&
                sandwichPrecio == model.sandwichPrecio;
        }
    }

}
