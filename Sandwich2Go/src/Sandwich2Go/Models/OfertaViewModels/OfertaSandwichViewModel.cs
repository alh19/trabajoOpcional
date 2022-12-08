using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sandwich2Go.Models.OfertaViewModels
{
    public class OfertaSandwichViewModel
    {
        public OfertaSandwichViewModel() {
        }
        public OfertaSandwichViewModel(Sandwich sandwich) : this()
        {
            SandwichID = sandwich.Id;
            Nombre = sandwich.SandwichName;
            Precio = sandwich.Precio;
            Ingredientes = new List<string>();
            foreach (IngredienteSandwich ingSand in sandwich.IngredienteSandwich)
            {
                Ingredientes.Add(ingSand.Ingrediente.Nombre + " ");
                IngM = IngM + ingSand.Ingrediente.Nombre + " ";
            }
        }

        public OfertaSandwichViewModel(OfertaSandwich ofertaSandwich) : this(ofertaSandwich.Sandwich)
        {
            Porcentaje = ofertaSandwich.Porcentaje;
        }
        public virtual string IngM { get; set; }
        public virtual int SandwichID { get; set; }
        public virtual string Nombre { get; set; }
        [DataType(DataType.Currency)]
        public virtual double Precio { get; set; }
        [Required]
        public virtual double Porcentaje { get; set; }
        public virtual List<string> Ingredientes { get; set; }

        public override bool Equals(object obj)
        {
            return obj is OfertaSandwichViewModel model &&
                SandwichID == model.SandwichID &&
                Nombre == model.Nombre &&
                Precio == model.Precio &&
                Porcentaje == model.Porcentaje &&
                Ingredientes.SequenceEqual(model.Ingredientes);
        }
    }
}
