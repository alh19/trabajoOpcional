using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SelectSandwichesViewModel
    {
        public IEnumerable<Sandwich> Sandwiches { get; set; }
        //Utilizado para filtrar por Alergeno
        public SelectList Alergenos;
        [Display(Name = "Alergeno")]
        public string sandwichAlergenosSelected { get; set; }
        //Utilizado para filtrar por precio del Sándwich
        [Display(Name = "Precio")]
        public float sandwichPrecio { get; set; }
        public SelectList Ingredientes;
    }
}
