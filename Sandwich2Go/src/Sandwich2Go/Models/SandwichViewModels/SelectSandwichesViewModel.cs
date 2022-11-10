using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SelectSandwichesViewModel
    {
        public IEnumerable<SandwichForPurchaseViewModel> Sandwiches { get; set; }
        //Utilizado para filtrar por Alergeno
        public SelectList Alergenos;
        [Display(Name = "Alérgeno: ")]
        public string sandwichAlergenoSelected { get; set; }
        //Utilizado para filtrar por precio del Sándwich
        [Display(Name = "Precio menor que: ")]
        public double sandwichPrecio { get; set; }


        public override bool Equals(object obj)
        {
            return obj is SelectSandwichesViewModel model &&
                Sandwiches.SequenceEqual(model.Sandwiches) &&
                Alergenos.Select(a => a.Value).SequenceEqual(model.Alergenos.Select(al => al.Value)) &&
                sandwichAlergenoSelected == model.sandwichAlergenoSelected &&
                sandwichPrecio == model.sandwichPrecio;
        }
    }
}
