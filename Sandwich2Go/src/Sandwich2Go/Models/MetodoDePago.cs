﻿using Sandwich2Go.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Sandwich2Go.Models
{

    public abstract class MetodoDePago
    {
        [Required]
        public virtual int Id { get; set; }

        [Required]
        public virtual IList<Pedido> Pedidos { get; set; }
    }
}