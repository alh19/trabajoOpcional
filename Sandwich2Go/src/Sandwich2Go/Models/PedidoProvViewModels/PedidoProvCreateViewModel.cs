using Sandwich2Go.Models.PedidoViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IngrPedProv = Sandwich2Go.Models.IngrPedProv;
using IngrPedProvViewModel = Sandwich2Go.Models.IngrPedProv;
using System.Linq;
using System.Xml.Linq;

namespace Sandwich2Go.Models.PedidoProvViewModels
{
    public class PedidoProvCreateViewModel : IValidatableObject
    {
        public PedidoProvCreateViewModel(int Id, string Cif, string NombreProveedor, string Direccion, int Cantidad, int IdProveedor,
            IList<IngrPedProvViewModel> ingredientesPedProv, string DireccionEntrega, string MetodoPago, string DireccionEnvio, DateTime FechaPedido, double PrecioTotal)
        {
            this.Id = Id;
            this.Cif = Cif;
            this.NombreProveedor = NombreProveedor;
            this.Direccion = Direccion;
            this.Cantidad = Cantidad;
            this.IdProveedor = IdProveedor;
            this.ingredientesPedProv = ingredientesPedProv;
            this.DireccionEntrega = DireccionEntrega;
            this.MetodoPago = MetodoPago;
            this.DireccionEnvio = DireccionEnvio;
            this.FechaPedido = FechaPedido;
            this.PrecioTotal = PrecioTotal;
        }

        public PedidoProvCreateViewModel()
        {
            ingredientesPedProv = new List<IngrPedProvViewModel>();
        }

        public PedidoProvCreateViewModel(PedidoProv pedidoprov)
        {
            this.Id = pedidoprov.Id;
            this.PrecioTotal = pedidoprov.PrecioTotal;
            this.DireccionEnvio = pedidoprov.DireccionEnvio;
            this.FechaPedido = pedidoprov.FechaPedido;
            //Gerente
            //this.ingredientesPedProv = pedidoprov.IngrPedProv;
            this.MetodoPago = pedidoprov.MetodoDePago.ToString();
        }

        public PedidoProvCreateViewModel(int id1, string cif, string nombre, string direccion1, int cantidad, int id2, object v1, string direccion2, string v2, string direccionEnvio, DateTime fechaPedido, double precioTotal)
        {
            Cif = cif;
            Cantidad = cantidad;
            DireccionEnvio = direccionEnvio;
            FechaPedido = fechaPedido;
            PrecioTotal = precioTotal;
        }

        public PedidoProvCreateViewModel(IList<PedidoProvCreateViewModel> expectedPurchaseItems)
        {
        }

        public virtual int Id { get; set; }
        public virtual string Cif { get; set; }
        public virtual string NombreProveedor { get; set; }
        public virtual string Direccion { get; set; }
        public virtual int Cantidad { get; set; }
        public virtual int IdProveedor { get; set; }
        public virtual bool necesitaCambio { get; set; }
        public IList<IngrPedProvViewModel> ingredientesPedProv { get; set; }


        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección de entrega:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduce una dirección de entrega.")]

        [StringLength(30, ErrorMessage = "No introduzcas una dirección mayor de 30 caracteres.")]
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

        public virtual string DireccionEnvio { get; set; }
        public virtual DateTime FechaPedido { get; set; }
        public virtual double PrecioTotal { get; set; }

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

            foreach (IngrPedProvViewModel s in ingredientesPedProv)
            {
                if (s.Equals(0))
                {
                    yield return new ValidationResult("Debes comprar al menos un ingrediente");
                }
            }

            if (ingredientesPedProv.Sum(pi => pi.Stock) <= 0)
                yield return new ValidationResult("Por favor, selecciona un stock mayor que 0",
                     new[] { nameof(ingredientesPedProv) });
        }


        public override bool Equals(object obj)
        {
            return obj is PedidoProvCreateViewModel pedidoProv &&
                Cif == pedidoProv.Cif &&
                NombreProveedor == pedidoProv.NombreProveedor &&
                IdProveedor == pedidoProv.IdProveedor &&
                Cantidad == pedidoProv.Cantidad &&
                Direccion == pedidoProv.Direccion &&
                DireccionEntrega == pedidoProv.DireccionEntrega &&
                MetodoPago == pedidoProv.MetodoPago &&
                Prefijo == pedidoProv.Prefijo &&
                Telefono == pedidoProv.Telefono &&
                NumeroTarjetaCredito == pedidoProv.NumeroTarjetaCredito &&
                CCV == pedidoProv.CCV &&
                ingredientesPedProv.SequenceEqual(pedidoProv.ingredientesPedProv) &&
                MesCad == pedidoProv.MesCad &&
                AnoCad == pedidoProv.AnoCad;
        }


        public class IngrPedProvViewModel
        {
            public IngrPedProvViewModel() { }

            public IngrPedProvViewModel(Ingrediente ingrPedido)
            {
                this.Id = ingrPedido.Id;
                this.NombreIngrediente = ingrPedido.Nombre;
                this.PrecioUnitario = ingrPedido.PrecioUnitario;
                this.Stock = ingrPedido.Stock;
                this.Alergenos = new List<string>();

                foreach (IngredienteSandwich ingSand in ingrPedido.IngredienteSandwich)
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
            }

            [DataType(DataType.Currency)]
            public virtual int Id { get; set; }
            public virtual string NombreIngrediente { get; set; }
            public virtual int PrecioUnitario
            {
                get;
                set;
            }
            public virtual int Stock { get; set; }
            public virtual int Cantidad { get; set; }
            public virtual IList<string> Alergenos { get; set; }
            public virtual IList<string> Ingredientes
            {
                get; set;
            }

            public virtual string Alm { get; set; }
            public virtual string IngM { get; set; }
            //public virtual IList<int> IdsIngrProv { get; set; }

            public override bool Equals(object obj)
            {
                return obj is IngrPedProvViewModel model &&
                    this.Id == model.Id &&
                    this.NombreIngrediente == model.NombreIngrediente &&
                    this.Stock == model.Stock &&
                    this.Cantidad == model.Cantidad &&
                    this.PrecioUnitario == model.PrecioUnitario &&
                    this.Ingredientes.SequenceEqual(model.Ingredientes) &&
                    this.Alergenos.SequenceEqual(model.Alergenos) &&
                    //this.IdsIngrProv.SequenceEqual(model.IdsIngrProv) &&
                    this.Alm == model.Alm &&
                    this.IngM == model.IngM;
            }

        }
    }
}