using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.IngredienteViewModels
{
    public class SelectedIngredientesForPurchaseViewModel
    {


        public string[] IdsToAdd { get; set; }
        [Display(Name = "Alergeno")]
        public string ingredienteAlergenoSelected { get; set; }
       
        [Display(Name = "Nombre")]
        public string ingredienteNombre
        {
            get; set;
        }
    }
}
