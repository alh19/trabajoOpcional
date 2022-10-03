using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Design
{
    public class MesaReserva
    {
        [Key]
        public virtual int Id { get; set; }
        [Required, DataType(DataType.Date), Display(Name = "Fecha de reserva")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaReserva { get; set; }
        [Required]
        public virtual int NumPersonas { get; set; }
        [ForeignKey("MesaId")]
        public virtual Mesa Mesa { get; set; }
        public virtual int MesaId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }
        public virtual string ClienteId { get; set; }
    }
}
