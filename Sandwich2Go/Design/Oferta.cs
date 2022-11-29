﻿using Humanizer.Localisation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Design
{
    public class Oferta
    {
        [Key]
        public virtual int Id { get; set; }
        [Required, StringLength(20, ErrorMessage = "El nombre no puede contener más de 20 caracteres")]
        public virtual string Nombre { get; set; }
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha de inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaInicio { get; set; }
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha de finalización")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaFin { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual IList<OfertaSandwich> OfertaSandwich { get; set; }
        [Required]
        public virtual Gerente Gerente { get; set; }
    }
}
