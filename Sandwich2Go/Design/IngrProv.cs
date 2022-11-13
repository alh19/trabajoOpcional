﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Design
{
    public class IngrProv
    {

        [Key]
        public virtual int Id { get; set; }

        [ForeignKey("IngredienteId")]
        public virtual Ingrediente Ingrediente { get; set; }
        public virtual int IngredienteId { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Proveedor Proveedor { get; set; }
        public virtual int ProveedorId { get; set; }

        [Required]
        public virtual IngrPedProv IngrPedProv { get; set; }
        //public virtual IList<IngrPedProv> IngrPedProv { get; set; }
    }
}
