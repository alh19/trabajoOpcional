using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime fecha { get; set; }

    }
}
