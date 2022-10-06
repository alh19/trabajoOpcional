using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandwich2Go.Models
{
    public class Efectivo : MetodoDePago
    {
        public virtual bool NecesitasCambio
        {
            get;
            set;
        }
    }
}