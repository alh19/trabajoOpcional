using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Design
{
    public class Tarjeta : MetodoDePago
    {
        [Required]
        public int Numero
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
    }
}