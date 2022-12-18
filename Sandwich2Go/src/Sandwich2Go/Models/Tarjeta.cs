using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sandwich2Go.Models
{
    public class Tarjeta : MetodoDePago
    {
        [Required]
        public long Numero
        {
            get;
            set;
        }
        [Required]
        public String Titular 
        {
            get;
            set;
        }
        [Required]
        public int CCV
        {
            get;
            set;
        }
        [Required]
        public int MesCaducidad
        {
            get;
            set;
        }
        [Required]
        public int AnoCaducidad
        {
            get;
            set;
        }

        public static implicit operator string(Tarjeta v)
        {
            throw new NotImplementedException();
        }
    }
}