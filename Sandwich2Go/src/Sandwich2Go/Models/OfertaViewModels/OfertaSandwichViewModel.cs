using Design;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sandwich2Go.Models.OfertaViewModels
{
    public class OfertaSandwichViewModel
    {
        public OfertaSandwichViewModel()
        {
            Porcentaje = 0.0;
        }
        public OfertaSandwichViewModel(OfertaSandwich ofertaSandwich)
        {
            Porcentaje = ofertaSandwich.Porcentaje;
            SandwichID = ofertaSandwich.SandwichId;
            Nombre = ofertaSandwich.Sandwich.SandwichName;
            Precio = ofertaSandwich.Sandwich.Precio;
            Descripcion = ofertaSandwich.Sandwich.Desc;
            Ingredientes = new List<string>();
            foreach (IngredienteSandwich ingSand in ofertaSandwich.Sandwich.IngredienteSandwich)
            {
                Ingredientes.Add(ingSand.Ingrediente.Nombre);
            }
        }
        public OfertaSandwichViewModel(Sandwich sandwich)
        {
            SandwichID = sandwich.Id;
            Nombre = sandwich.SandwichName;
            Precio = sandwich.Precio;
            Descripcion = sandwich.Desc;
            Ingredientes = new List<string>();
            foreach (IngredienteSandwich ingSand in sandwich.IngredienteSandwich)
            {
                Ingredientes.Add(ingSand.Ingrediente.Nombre);
            }
        }
        public virtual int SandwichID { get; set; }
        public virtual string Nombre { get; set; }
        public virtual double Precio { get; set; }
        [Required]
        public virtual double Porcentaje { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual List<string> Ingredientes { get; set; }

        public override bool Equals(object obj)
        {
            return obj is OfertaSandwichViewModel model &&
                SandwichID == model.SandwichID &&
                Porcentaje == model.Porcentaje &&
                Nombre == model.Nombre &&
                Precio == model.Precio &&
                Descripcion == model.Descripcion &&
                Ingredientes.SequenceEqual(model.Ingredientes);
        }

    }
}
