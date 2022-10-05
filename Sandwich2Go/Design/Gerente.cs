using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Gerente : Usuario
    {
        [Required]
        [DataType(DataType.Date), Display(Name = "Fecha de contratación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual string FechaContratacion { get; set; }
        [Required, DataType(DataType.Currency)]
        public virtual double Salario { get; set; }
        public IList<OfertaGerente> OfertaGerente { get; set; }
    }
}
