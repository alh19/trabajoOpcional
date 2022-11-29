using System.ComponentModel.DataAnnotations;
using System;

namespace Sandwich2Go.Models.OfertaViewModels
{
    public class OfertaIndexViewModel
    {
        public OfertaIndexViewModel(Oferta oferta)
        {
            Id = oferta.Id;
            NombreOferta = oferta.Nombre;
            FechaInicio = oferta.FechaInicio;
            FechaFin = oferta.FechaFin;
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

    }
}
