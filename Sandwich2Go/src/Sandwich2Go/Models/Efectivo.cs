using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandwich2Go.Models
{
    public class Efectivo : MetodoDePago
    {
        public bool NecesitasCambio
        {
            get;
            set;
        }

        public static implicit operator string(Efectivo v)
        {
            throw new NotImplementedException();
        }
    }
}