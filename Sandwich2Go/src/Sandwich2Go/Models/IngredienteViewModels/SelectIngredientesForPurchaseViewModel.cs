using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;

namespace Sandwich2Go.Models.IngredienteViewModels
{
    public class SelectIngredientesForPurchaseViewModel
    {
        public IEnumerable<IngredienteForPurchaseViewModel> Ingredientes { get; set; }
        //Utilizado para filtrar por Alergeno
        public SelectList Alergenos;
        [Display(Name = "Alergeno")]
        public string ingredienteAlergenoSelected { get; set; }
        //Utilizado para filtrar por nombre del ingrediente
        [Display(Name = "Nombre")]
        public string ingredienteNombre
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            return obj is SelectIngredientesForPurchaseViewModel model &&
                Ingredientes.SequenceEqual(model.Ingredientes) &&
                ingredienteAlergenoSelected == model.ingredienteAlergenoSelected &&
                ingredienteNombre == model.ingredienteNombre;
                Alergenos = model.Alergenos;
        }
    }
}
