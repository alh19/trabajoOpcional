using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Sandwich2Go.Models
{
    public class Alergeno
    {
        [Key]
        public virtual int id { get; set; }
        [Required, StringLength(20, ErrorMessage = "El nombre no puede ser mayor a 20 caracteres.")]
        public virtual string Name { get; set; }

        public virtual IList<AlergSandw> AlergSandws { get; set; }

        public override bool Equals(object obj)
        {
            
            return obj is Alergeno alergeno &&
                this.id == alergeno.id &&
                this.Name.Equals(alergeno.Name) &&
                this.AlergSandws.Equals(alergeno.AlergSandws);
        }
    }
}
