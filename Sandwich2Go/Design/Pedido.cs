using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Design;

namespace Design
{
    public class Pedido
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual string Nombre { get; set; }


        [Required]
        [DataType(DataType.Date), Display(Name = "DateTime")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime Fecha { get; set; }


        [Required, Display(Name = "preciototal")]
        [Range(1, int.MaxValue, ErrorMessage = "El precio mínimo es de 1")]
        public virtual int Preciototal { get; set; }


        [Required, StringLength(30, ErrorMessage = "First name cannot be longer than 30 characters.")]
        public virtual string Direccion { get; set; }


        [Required]
        public virtual string Descripcion{ get; set; }


        [Required]
        public virtual int Cantidad { get; set; }

        public virtual IList<SandwichPedido> sandwichesPedidos
        {
            get;
            set;
        }
        [Required]
        public virtual Cliente Cliente
        {
            get;
            set;
        }
        [Required]
        public MetodoDePago MetodoDePago
        {
            get;
            set;
        }

        [Required]
        public SandwCreado SandwCreado
        {
            get;
            set;
        }
    }
}
