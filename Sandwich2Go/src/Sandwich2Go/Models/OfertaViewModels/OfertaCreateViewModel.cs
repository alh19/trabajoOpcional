using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;
using System;
using Xunit.Sdk;

namespace Sandwich2Go.Models.OfertaViewModels
{
    public class OfertaCreateViewModel
    {
        public OfertaCreateViewModel() { }

        public OfertaCreateViewModel(Gerente gerente, IList<OfertaSandwichViewModel> ofertaSandwiches)
        {
            Nombre = gerente.Nombre;
            Apellido = gerente.Apellido;
            Email = gerente.Email;
            OfertaSandwiches = ofertaSandwiches;
        }

        [Display(Name = "Nombre")]
        public virtual string Nombre { get; set; }
        [Display(Name = "Apellido")]
        public virtual string Apellido { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public virtual string Email { get; set; }
        public virtual int GerenteId { get; set; }
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
              Apellido == model.Apellido &&
              GerenteId == model.GerenteId &&
              Email == model.Email &&
              NombreOferta == model.NombreOferta &&
              FechaInicio == model.FechaInicio &&
              FechaFin == model.FechaFin &&
              Descripcion == model.Descripcion &&
              OfertaSandwiches.SequenceEqual(model.OfertaSandwiches);
        }
    }
}