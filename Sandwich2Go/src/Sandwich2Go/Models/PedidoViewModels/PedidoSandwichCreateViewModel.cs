using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sandwich2Go.Models.PedidoViewModels
{
    public class PedidoSandwichCreateViewModel : IValidatableObject
    {
        public PedidoSandwichCreateViewModel() { }

        public virtual string Name{ get; set; }
        public virtual string Apellido { get; set; }
        public virtual string IdCliente { get; set; }
        [DataType(DataType.Currency)]
        public virtual double PrecioTotal { get; set; }
        public DateTime fechaCompra { get; set; }
        public IList<SandwichPedidoViewModel> sandwichesPedidos { get; set; }

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
        public virtual double PrecioFinal { get; set; } 

        [StringLength(9, MinimumLength = 9)]

        public string Telefono { get; set; }

        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Debes introducir los 16 dígitos de la tarjeta")]
        [Display(Name = "Tarjeta de Crédito")]
        public virtual string NumeroTarjetaCredito { get; set; }

        [RegularExpression(@"^[0-9]{3}$")]
        public virtual string CCV { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/yyyy}")]
        public virtual DateTime? FechaCaducidad { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(MetodoPago == "Tarjeta")
            {
                if (NumeroTarjetaCredito == null)
                    yield return new ValidationResult("Por favor, rellena el número de la tarjeta de crédito", 
                        new[] {nameof(NumeroTarjetaCredito)});
                if (CCV == null)
                    yield return new ValidationResult("Por favor, rellena el CCV de la tarjeta de crédito",
                        new[] { nameof(CCV) });
                if (FechaCaducidad == null)
                    yield return new ValidationResult("Por favor, rellena la fecha de caducidad de la tarjeta de crédito",
                        new[] { nameof(FechaCaducidad) });
            }
        }

        public virtual void precioTotal()
        {
            this.PrecioTotal = 0;
            this.PrecioFinal = 0;
            this.necesitaCambio = false;
            foreach(SandwichPedidoViewModel s in sandwichesPedidos)
            {
                this.PrecioTotal += (s.PrecioCompra)*s.cantidad;
                this.PrecioFinal += (s.PrecioCompra - (s.PrecioCompra*(s.porcentajeOferta/100)))*s.cantidad;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is PedidoSandwichCreateViewModel pedido &&
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
                FechaCaducidad.Equals(pedido.FechaCaducidad) &&
                sandwichesPedidos.SequenceEqual(pedido.sandwichesPedidos);
        }

    }

}
