using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Oferta
    {
        [Key]
        public virtual int Id { get; set; }
        [Required, StringLength(20, ErrorMessage = "El nombre no puede contener más de 20 caracteres")]
        public virtual string Name { get; set; }
        [Required, DataType(DataType.Currency)]
        public virtual double PrecioTotal { get; set; }
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha de inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha de finalización")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaFin { get; set; }
        public IList<OfertaGerente> OfertaGerente { get; set; }
        public IList<OfertaSandwich> OfertaSandwich { get; set; }
    }
}
