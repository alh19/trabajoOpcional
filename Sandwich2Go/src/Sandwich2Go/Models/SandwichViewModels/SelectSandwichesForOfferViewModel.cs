using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SelectSandwichesForOfferViewModel
    {
        public IEnumerable<Sandwich> Sandwiches { get; set; }
        //Utilizado para filtrar por nombre del Sándwich
        [Display(Name = "Nombre")]
        public string sandwichNombre { get; set; }
        //Utilizado para filtrar por precio del Sándwich
        [Display(Name = "Precio")]
        public float sandwichPrecio { get; set; }
    }
}
