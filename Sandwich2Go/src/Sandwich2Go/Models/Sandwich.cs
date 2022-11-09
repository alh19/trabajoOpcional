﻿using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Humanizer.Localisation;
using System.Linq;

namespace Sandwich2Go.Models
{
    public class Sandwich
    {
        [Key]
        public virtual int Id { get; set; }
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

        public override bool Equals(object obj)
        {
            Sandwich sandwich = obj as Sandwich;
            List<IngredienteSandwich> inter = this.IngredienteSandwich.Union(sandwich.IngredienteSandwich).ToList();
            List<IngredienteSandwich> union = this.IngredienteSandwich.Intersect(sandwich.IngredienteSandwich).ToList();

            return true;/*obj is Sandwich sandwich &&
                this.Id == sandwich.Id &&
                this.SandwichName.Equals(sandwich.SandwichName) &&
                this.Precio == sandwich.Precio &&
                this.Desc.Equals(sandwich.Desc) &&
                (this.IngredienteSandwich.Union(sandwich.IngredienteSandwich)).Equals(this.IngredienteSandwich.Intersect(sandwich.IngredienteSandwich));*/
            
        }
    }
}
