﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class OfertaSandwich
    {
        [ForeignKey("OfertaId")]
        public virtual Oferta Oferta { get; set; }
        public virtual int OfertaId { get; set; }

        [ForeignKey("SandwichId")]
        public virtual Sandwich Sandwich { get; set; }
        public virtual int SandwichId { get; set; }
        [Required]
        public virtual double Porcentaje { get; set; }
    }
}
