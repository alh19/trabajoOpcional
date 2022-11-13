using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sandwich2Go.Models.ProveedorViewModels
{
    public class ProveedorForPurchaseViewModel
    {
        public ProveedorForPurchaseViewModel()
        {

        }

        public ProveedorForPurchaseViewModel(Proveedor proveedor)
        {
            Id = proveedor.Id;
            Nombre = proveedor.Nombre;
            Cif = proveedor.Cif;
            Direccion = proveedor.Direccion;
        }

        [Key]
        public int Id { get; set; }


        [Required, StringLength(20, ErrorMessage = "First name cannot be longer than 20 characters.")]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Cif")]
        public string Cif { get; set; }

        [Required, Display(Name = "Dirección")]
        public string Direccion { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ProveedorForPurchaseViewModel model &&
                Id == model.Id &&
                Nombre == model.Nombre &&
                Cif == model.Cif &&
                Direccion == model.Direccion;
        }
    }
}
