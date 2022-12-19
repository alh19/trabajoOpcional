using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace Sandwich2Go.Models.OfertaViewModels
{
    public class OfertaDetailsViewModel
    {
        public OfertaDetailsViewModel(Oferta oferta)
        {
            Id = oferta.Id;
            NombreOferta = oferta.Nombre;
            FechaInicio = oferta.FechaInicio;
            FechaFin = oferta.FechaFin;
            Descripcion = oferta.Descripcion;
            OfertaSandwiches = oferta.OfertaSandwich
                .Select(pi => new OfertaSandwichDetailsViewModel(pi)).ToList();
        }
        public int Id { get; set; }
        [Display(Name = "Nombre de la oferta: ")]
        public string NombreOferta { get; set; }
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha de inicio: ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha de finalización: ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaFin { get; set; }
        [Display(Name = "Descripción: ")]
        public virtual string Descripcion { get; set; }
        public virtual IList<OfertaSandwichDetailsViewModel> OfertaSandwiches { get; set; }

        public override bool Equals(object? obj)
        {
            var result = true;
            //  return obj is PurchaseDetailsViewModel model &&
            var model = obj as OfertaDetailsViewModel;
            result = result &&
              Id == model.Id &&
              NombreOferta == model.NombreOferta &&
              (this.FechaInicio.Subtract(model.FechaInicio) < new TimeSpan(0, 1, 0)) &&
              (this.FechaFin.Subtract(model.FechaFin) < new TimeSpan(0, 1, 0)) &&
              Descripcion == model.Descripcion &&
              OfertaSandwiches.SequenceEqual(model.OfertaSandwiches);
            return result;
        }
    }

    public class OfertaSandwichDetailsViewModel
    {

        public OfertaSandwichDetailsViewModel()
        {

        }
        public OfertaSandwichDetailsViewModel(Sandwich sandwich) : this()
        {
            Nombre = sandwich.SandwichName;
            Precio = sandwich.Precio;
            Ingredientes = new List<string>();
            foreach (IngredienteSandwich isa in sandwich.IngredienteSandwich)
            {
                Ingredientes.Add(isa.Ingrediente.Nombre + " ");
            }
        }
        public OfertaSandwichDetailsViewModel(OfertaSandwich ofertaSandwich) : this(ofertaSandwich.Sandwich)
        {
            Porcentaje = ofertaSandwich.Porcentaje;
        }

        public string Nombre { get; set; }

        public IList<string> Ingredientes { get; set; }

        public double Porcentaje { get; set; }

        [DataType(DataType.Currency)]
        public virtual double Precio { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is OfertaSandwichDetailsViewModel model &&
                   Nombre == model.Nombre &&
                   Porcentaje == model.Porcentaje &&
                   Ingredientes.SequenceEqual(model.Ingredientes) &&
                   Precio == model.Precio;
        }
    }
}
