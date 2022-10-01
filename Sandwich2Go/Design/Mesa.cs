﻿using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Mesa
    {
        [Key]
        public virtual int MesaId { get; set; }
        [Required]
        public virtual int Capacidad { get; set; }
        [Required]
        public virtual string Estado { get; set; }
    }
}
