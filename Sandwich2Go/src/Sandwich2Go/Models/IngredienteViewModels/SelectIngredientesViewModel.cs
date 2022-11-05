using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.IngredienteViewModels
{
    public class SelectIngredientesViewModel
    {
        public IEnumerable<Ingrediente> Ingredientes { get; set; }
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
    }
}
