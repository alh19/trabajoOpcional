using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;
using System;
using Xunit.Sdk;

namespace Sandwich2Go.Models.OfertaViewModels
{
    public class OfertaCreateViewModel : IValidatableObject
    {
        public OfertaCreateViewModel(Oferta oferta)
        {
            Nombre = oferta.Gerente.Nombre;
            Apellido = oferta.Gerente.Apellido;
            GerenteId = oferta.Gerente.Id;
            Email = oferta.Gerente.Email;
            Salario = oferta.Gerente.Salario;
            FechaContratacion = oferta.Gerente.FechaContratacion;
            Direccion = oferta.Gerente.Direccion;
            Telefono = oferta.Gerente.PhoneNumber;
            NombreOferta = oferta.Nombre;
            FechaInicio = oferta.FechaInicio;
            FechaFin = oferta.FechaFin;
            Descripcion = oferta.Descripcion;
            OfertaSandwiches = oferta.OfertaSandwich.Select(pi => new OfertaSandwichViewModel(pi)).ToList();
        }
        public OfertaCreateViewModel(Gerente gerente, IList<OfertaSandwichViewModel> ofertaSandwiches)
        {
            Nombre = gerente.Nombre;
            Apellido = gerente.Apellido;
            Email = gerente.Email;
            Salario = gerente.Salario;
            FechaContratacion = gerente.FechaContratacion;
            Direccion = gerente.Direccion;
            Telefono = gerente.PhoneNumber;
            OfertaSandwiches = ofertaSandwiches;
        }

        public virtual string Nombre { get; set; }

        public virtual string Apellido { get; set; }
        [EmailAddress]
        public virtual string Email { get; set; }
        
        public virtual double Salario { get; set; }

        [DataType(DataType.Date), Display(Name = "Fecha de contratación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual string FechaContratacion { get; set; }

        public virtual string Direccion { get; set; }
        public virtual string Telefono { get; set; }
        public string GerenteId { get; set; }
        [Required, StringLength(20, ErrorMessage = "El nombre no puede contener más de 20 caracteres")]
        public string NombreOferta { get; set; }
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha de inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha de finalización")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaFin { get; set; }
        [Required]
        public virtual string Descripcion { get; set; }
        public virtual IList<OfertaSandwichViewModel> OfertaSandwiches { get; set; }


        public OfertaCreateViewModel()
        {
            OfertaSandwiches = new List<OfertaSandwichViewModel>();
        }

        public override bool Equals(object obj)
        {
            return obj is OfertaCreateViewModel model &&
              Nombre == model.Nombre &&
              Apellido == model.Apellido &&
              GerenteId == model.GerenteId &&
              Email == model.Email &&
              Salario == model.Salario &&
              FechaContratacion == model.FechaContratacion &&
              Direccion == model.Direccion &&
              Telefono == model.Telefono &&
              NombreOferta == model.NombreOferta &&
              FechaInicio == model.FechaInicio &&
              FechaFin == model.FechaFin &&
              Descripcion == model.Descripcion &&
              OfertaSandwiches.SequenceEqual(model.OfertaSandwiches);
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OfertaSandwiches.Sum(pi => pi.Porcentaje) <= 0.0)
                yield return new ValidationResult("Por favor, Selecciona un porcentaje mayor que 0 para almenos un sandwich.",
                     new[] { nameof(OfertaSandwiches) });
        }
    }
}