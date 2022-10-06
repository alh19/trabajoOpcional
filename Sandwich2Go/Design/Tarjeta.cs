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
        public virtual int Numero
        {
            get;
            set;
        }
        [Required]
        public virtual String Titular
        {
            get;
            set;
        }
        [Required]
        public virtual int CCV
        {
            get;
            set;
        }
        [Required]
        public virtual int MesCaducidad
        {
            get;
            set;
        }
        [Required]
        public virtual int AnoCaducidad
        {
            get;
            set;
        }
    }
}