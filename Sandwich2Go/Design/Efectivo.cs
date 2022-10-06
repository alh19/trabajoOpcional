using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Design
{
    public class Efectivo : MetodoDePago
    {
        public bool NecesitasCambio
        {
            get;
            set;
        }
    }
}