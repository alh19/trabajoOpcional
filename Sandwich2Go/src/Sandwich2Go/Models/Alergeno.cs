using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Sandwich2Go.models
{
    public class Alergeno
    {
        [Key]
        public virtual int id { get; set; }
        [Required, StringLength(20, ErrorMessage = "El nombre no puede ser mayor a 20 caracteres.")]
        public virtual string name { get; set; }

    }
}
