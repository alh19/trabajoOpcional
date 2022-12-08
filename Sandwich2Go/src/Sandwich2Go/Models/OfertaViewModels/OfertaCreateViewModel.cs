using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;

namespace Sandwich2Go.Models.OfertaViewModels
{
    public class OfertaCreateViewModel : IValidatableObject
    {
        public OfertaCreateViewModel() 
        {
        }

        public OfertaCreateViewModel(Gerente gerente, IList<OfertaSandwichViewModel> ofertaSandwiches)
        {
            Nombre = gerente.Nombre;
            Apellido = gerente.Apellido;
            Email = gerente.Email;
            OfertaSandwiches = ofertaSandwiches;
        }
        public OfertaCreateViewModel(Oferta oferta)
        {
            Nombre = oferta.Gerente.Nombre;
            Apellido = oferta.Gerente.Apellido;
            Email = oferta.Gerente.Email;
            GerenteId = oferta.Gerente.Id;
            NombreOferta = oferta.Nombre;
            FechaInicio = oferta.FechaInicio;
            FechaFin = oferta.FechaFin;
            Descripcion = oferta.Descripcion;
            OfertaSandwiches = oferta.OfertaSandwich.Select(pi => new OfertaSandwichViewModel(pi)).ToList();
        }

        [Display(Name = "Nombre:")]
        public virtual string Nombre { get; set; }
        [Display(Name = "Apellido:")]
        public virtual string Apellido { get; set; }
        [EmailAddress]
        [Display(Name = "Email:")]
        public virtual string Email { get; set; }
        public virtual string GerenteId { get; set; }
        [Required, StringLength(20, ErrorMessage = "El nombre no puede contener más de 20 caracteres")]
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
        public virtual IList<OfertaSandwichViewModel> OfertaSandwiches { get; set; }

        public override bool Equals(object obj)
        {
            return obj is OfertaCreateViewModel model &&
              Nombre == model.Nombre &&
              FechaInicio == model.FechaInicio &&
              GerenteId == model.GerenteId &&
              Email == model.Email &&
              NombreOferta == model.NombreOferta &&
              (this.FechaInicio.Subtract(model.FechaInicio) < new TimeSpan(0, 1, 0)) &&
              (this.FechaFin.Subtract(model.FechaFin) < new TimeSpan(0, 1, 0)) &&
              Descripcion == model.Descripcion &&
              OfertaSandwiches.SequenceEqual(model.OfertaSandwiches);
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime.Compare(FechaFin, FechaInicio) < 0)
            {
                yield return new ValidationResult("La fecha de finalización tiene que ser mayor o igual que la fecha de inicio",
                     new[] { nameof(FechaFin) });
            }
            if (DateTime.Compare(FechaInicio, DateTime.Today) < 0)
            {
                yield return new ValidationResult("La fecha de inicio tiene que ser mayor o igual que la fecha de hoy",
                     new[] { nameof(FechaInicio) });
            }
        }
    }
}
