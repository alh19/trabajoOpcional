﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandwich2Go.Models
{
    public class IngrPedProv
    {
        [Key]
        public virtual int Id { get; set; }
        [Required, Display(Name = "cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
        public virtual int Cantidad { get; set; }
        public virtual int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public virtual PedidoProv PedidoProv { get; set; }
        public virtual int PedidoProvId { get; set; }
        [ForeignKey("IngrProvId")]
        public virtual IngrProv IngrProv { get; set; }
        public virtual int IngrProvId { get; set; }

    }
}
