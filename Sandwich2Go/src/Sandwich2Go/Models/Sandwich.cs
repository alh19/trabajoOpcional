﻿using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Sandwich2Go.Models
{
    public class Sandwich
    {
        [Key]
        public virtual string Id { get; set; }
        [Required, StringLength(20, ErrorMessage = "El nombre no puede ser mayor a 20 caracteres.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public virtual string SandwichName { get; set;}
        [Required, DataType(DataType.Currency)]//Tipo moneda
        public virtual double Precio { get; set; }
        [Required,StringLength(100, ErrorMessage = "La descripción no puede ser mayor a 100 caracteres.")]
        public virtual string Desc { get; set; }

        public virtual IList<SandwichPedido> SandwichPedido
        {
            get;
            set;
            
            
        }

        public virtual IList<IngredienteSandwich> IngredienteSandwich
        {
            get;
            set;
        }
        public virtual IList<OfertaSandwich> OfertaSandwich { get; set; }
    }
}
