using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sandwich2Go.Models.PedidoViewModels
{
    public class PedidoSandwichCreateViewModel : IValidatableObject
    {
        
        public virtual string Name{ get; set; }
        public virtual string Apellido { get; set; }
        public virtual string IdCliente { get; set; }
        [DataType(DataType.Currency)]
        public virtual double PrecioTotal { get; set; }
        public IList<SandwichPedidoViewModel> sandwichesPedidos { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección de entrega:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduce una dirección de entrega.")]
        
        [StringLength(30,ErrorMessage = "No introduzcas una dirección mayor de 30 caracteres.")]
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


        [StringLength(3, MinimumLength = 2)]
        public virtual string Prefijo { get; set; }

        [StringLength(9, MinimumLength = 9)]

        public string Telefono { get; set; }

        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Debes introducir los 16 dígitos de la tarjeta")]
        [Display(Name = "Tarjeta de Crédito")]
        public virtual string NumeroTarjetaCredito { get; set; }

        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "El formato son 3 dígitos.")]
        public virtual string CCV { get; set; }
        [Range(1, 12, ErrorMessage = "Introduce un mes del 1-12")]
        [Display(Name = "Mes Caducidad")]
        public virtual string MesCad { get; set; }
        [Range(2000, 2100, ErrorMessage = "Introduce un año válido")]
        [Display(Name = "Año caducidad")]
        public virtual string AnoCad { get; set; }

        public PedidoSandwichCreateViewModel()
        {
            sandwichesPedidos = new List<SandwichPedidoViewModel>();
        }

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
                if (MesCad == null)
                    yield return new ValidationResult("Por favor, rellena el mes de caducidad de la tarjeta de crédito",
                        new[] { nameof(MesCad) });
                if (AnoCad == null)
                    yield return new ValidationResult("Por favor, rellena el año de caducidad de la tarjeta de crédito",
                        new[] { nameof(AnoCad) });
            }

            foreach(SandwichPedidoViewModel s in sandwichesPedidos)
            {
                if(s.cantidad <= 0)
                {
                    yield return new ValidationResult("Debes comprar al menos un sandwich de cada tipo",
                        new[] { nameof(s.cantidad) });
                }
            }

        }

        public override bool Equals(object obj)
        {
            return obj is PedidoSandwichCreateViewModel pedido &&
                Name == pedido.Name &&
                Apellido == pedido.Apellido &&
                IdCliente == pedido.IdCliente &&
                PrecioTotal == pedido.PrecioTotal &&
                DireccionEntrega == pedido.DireccionEntrega &&
                MetodoPago == pedido.MetodoPago &&
                Prefijo == pedido.Prefijo &&
                Telefono == pedido.Telefono &&
                NumeroTarjetaCredito == pedido.NumeroTarjetaCredito &&
                CCV == pedido.CCV &&
                sandwichesPedidos.SequenceEqual(pedido.sandwichesPedidos) &&
                MesCad == pedido.MesCad &&
                AnoCad == pedido.AnoCad;
        }

    }
    public class SandwichPedidoViewModel
    {
        public SandwichPedidoViewModel() { }

        public SandwichPedidoViewModel(Sandwich sandwichPedido)
        {
            this.Id = sandwichPedido.Id;
            this.NombreSandwich = sandwichPedido.SandwichName;
            this.PrecioCompra = sandwichPedido.Precio;
            this.Ingredientes = new List<string>();
            this.Alergenos = new List<string>();
            this.porcentajeOferta = 0;
            this.cantidad = 1;
            this.Alm = "";
            this.PrecioConDescuento = -1;
            foreach (IngredienteSandwich ingSand in sandwichPedido.IngredienteSandwich)
            {
                this.Ingredientes.Add(ingSand.Ingrediente.Nombre + " ");
                this.IngM = this.IngM + ingSand.Ingrediente.Nombre + " ";
                foreach (AlergSandw alSand in ingSand.Ingrediente.AlergSandws)
                {
                    if (!this.Alergenos.Contains(alSand.Alergeno.Name + " "))
                    {
                        this.Alergenos.Add(alSand.Alergeno.Name + " ");
                        this.Alm = Alm + alSand.Alergeno.Name + " ";
                    }
                }
            }
            if (sandwichPedido.OfertaSandwich != null)
            {
                foreach (OfertaSandwich os in sandwichPedido.OfertaSandwich)
                {
                    if (os.Porcentaje > this.porcentajeOferta && os.Oferta.FechaFin > DateTime.Now)
                    {
                        this.oferta = os.Oferta.Nombre;
                        this.porcentajeOferta = os.Porcentaje;
                    }
                    this.descuento = this.PrecioCompra * (this.porcentajeOferta / 100);
                    this.oferta = this.NombreSandwich + " con oferta " + this.oferta + " y descuento del " + this.porcentajeOferta + "% ..... -";
                    this.PrecioConDescuento = this.PrecioCompra - this.descuento;

                    this.lineaDescuento = this.NombreSandwich + " con oferta " + os.Oferta.Nombre + " y descuento del " + this.porcentajeOferta + "% ..... -"+this.descuento.ToString("C")+" / Sandwich. Precio final: "+this.PrecioConDescuento.ToString("C");
                }
            }

        }
        [DataType(DataType.Currency)]
        public virtual double descuento { get; set; }
        public virtual int Id { get; set; }

        public virtual string oferta { get; set; }
        public virtual double porcentajeOferta { get; set; }
        public virtual string NombreSandwich
        {
            get; set;
        }
        [DataType(DataType.Currency)]
        public virtual double PrecioCompra
        {
            get; set;
        }
        [DataType(DataType.Currency)]
        public virtual double PrecioConDescuento
        {
            get; set;
        }
        [Range(1, 10, ErrorMessage = "Minimo un sándwich, máximo 10 de un mismo tipo por pedido")]
        public virtual int cantidad { get; set; }
        public virtual IList<string> Ingredientes
        {
            get; set;
        }

        public virtual IList<string> Alergenos
        {
            get; set;
        }

        public virtual string Alm { get; set; }
        public virtual string IngM { get; set; }

        public virtual string lineaDescuento { get; set; }
        public override bool Equals(object obj)
        {
            return obj is SandwichPedidoViewModel model &&
                this.Id == model.Id &&
                this.NombreSandwich == model.NombreSandwich &&
                this.PrecioCompra == model.PrecioCompra &&
                this.Ingredientes.SequenceEqual(model.Ingredientes) &&
                this.Alergenos.SequenceEqual(model.Alergenos) &&
                this.porcentajeOferta == model.porcentajeOferta &&
                this.oferta == model.oferta &&
                this.PrecioConDescuento == model.PrecioConDescuento &&
                this.descuento == model.descuento &&
                this.Alm == model.Alm &&
                this.IngM == model.IngM &&
                this.lineaDescuento == model.lineaDescuento;
        }

    }
}
