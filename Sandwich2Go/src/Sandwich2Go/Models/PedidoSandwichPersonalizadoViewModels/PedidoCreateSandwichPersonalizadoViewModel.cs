using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace Sandwich2Go.Models.PedidoSandwichPersonalizadoViewModels
{
    public class PedidoCreateSandwichPersonalizadoViewModel : IValidatableObject
    {

        public PedidoCreateSandwichPersonalizadoViewModel()
        {

        }

        public virtual string Name { get; set; }
        public virtual string Apellido { get; set; }
        public virtual string IdCliente { get; set; }
        [DataType(DataType.Currency)]
        public virtual double PrecioTotal { get; set; }
        public DateTime fechaCompra { get; set; }
        public IList<IngredientePedidoViewModel> ingPedidos { get; set; }
        public virtual string MesCad { get; set; }
        [Range(2000, 2100, ErrorMessage = "Introduce un año válido")]
        [Display(Name = "Año caducidad")]
        public virtual string AnoCad { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección de entrega:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]

        public string DireccionEntrega
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Método de pago")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, selecciona un método de pago")]

        public string MetodoPago
        {
            get;
            set;
        }
        public virtual bool necesitaCambio { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(3, MinimumLength = 2)]
        public virtual string Prefijo { get; set; }
        [DataType(DataType.Currency)]


        [Range(1, 100, ErrorMessage = "Introduce al menos uno")]
        public virtual int Cantidad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduce al menos uno")]
        [StringLength(9, MinimumLength = 9)]
        [RegularExpression("^[0-9]{9}", ErrorMessage = "Debes introducir los 9 dígitos del teléfono")]
        public string Telefono { get; set; }


        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Debes introducir los 16 dígitos de la tarjeta")]
        [Display(Name = "Tarjeta de Crédito")]
        public virtual string NumeroTarjetaCredito { get; set; }

        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "El formato son 3 dígitos.")]
        public virtual string CCV { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MetodoPago == "Tarjeta")
            {
                if (NumeroTarjetaCredito == null)
                    yield return new ValidationResult("Por favor, rellena el número de la tarjeta de crédito",
                        new[] { nameof(NumeroTarjetaCredito) });
                if (CCV == null)
                    yield return new ValidationResult("Por favor, rellena el CCV de la tarjeta de crédito",
                        new[] { nameof(CCV) });
                if (MesCad == null)
                    yield return new ValidationResult("Por favor, rellena el mes de caducidad de la tarjeta de crédito",
                        new[] { nameof(MesCad) });
                if (AnoCad == null)
                    yield return new ValidationResult("Por favor, rellena el año de caducidad de la tarjeta de crédito",
                        new[] { nameof(AnoCad) });
            }

            foreach (IngredientePedidoViewModel s in ingPedidos)
            {
                if (s.cantidad <= 0)
                {
                    yield return new ValidationResult("Debes comprar al menos un sandwich de cada tipo",
                        new[] { nameof(s.cantidad) });
                }
            }

        }

        public override bool Equals(object obj)
        {
            return obj is PedidoCreateSandwichPersonalizadoViewModel pedido &&
                Name == pedido.Name &&
                Apellido == pedido.Apellido &&
                IdCliente == pedido.IdCliente &&
                PrecioTotal == pedido.PrecioTotal &&
                fechaCompra.Equals(pedido.fechaCompra) &&
                DireccionEntrega == pedido.DireccionEntrega &&
                MetodoPago == pedido.MetodoPago &&
                Email == pedido.Email &&
                Prefijo == pedido.Prefijo &&
                Telefono == pedido.Telefono &&
                NumeroTarjetaCredito == pedido.NumeroTarjetaCredito &&
                CCV == pedido.CCV &&
                Cantidad == pedido.Cantidad &&
                AnoCad == pedido.AnoCad &&
                MesCad == pedido.MesCad &&
                ingPedidos.SequenceEqual(pedido.ingPedidos);
        }

    }
}
