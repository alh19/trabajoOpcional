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
        [ForeignKey("Mesa")]
        public virtual string MesaId { get; set; }
        public virtual Mesa Mesa { get; set; }
        [ForeignKey("Cliente")]
        public virtual string ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}
