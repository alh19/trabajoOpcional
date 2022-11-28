using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models.PedidoViewModels
{
    public class PedidoDetailsViewModel
    {
        public string nombrePedido { get; set; }
        [Display(Name = "Nombre:")]
        public string clienteNombre { get; set; }
        [Display(Name = "Apellidos:")]
        public string clienteApellidos { get; set; }
        [Display(Name = "Fecha de compra:")]
        public string fechaCompra { get; set; }
        [Display(Name = "Dirección de entrega:")]
        public string direccionEntrega { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Precio total:")]
        public double precioTotal { get; set; }
        [Display(Name = "Método de pago:")]
        public string metodoPago { get; set; }
        public IList<SandwichPedidoDetailsViewModel> sandwichesPedidos { get; set; }

        public PedidoDetailsViewModel(Pedido p)
        {
            nombrePedido = p.Nombre;
            clienteNombre = p.Cliente.Nombre;
            clienteApellidos = p.Cliente.Apellido;
            fechaCompra = p.Fecha.ToString();
            direccionEntrega = p.Direccion;
            precioTotal = p.Preciototal;
            sandwichesPedidos = new List<SandwichPedidoDetailsViewModel>();
            MetodoPagoDetailsViewModel metodo = new MetodoPagoDetailsViewModel(p.MetodoDePago);
            if (metodo.Tipo == "Tarjeta")
            {
                metodoPago = "Pagado con tarjeta *" + metodo.Tarjeta;
            }
            else
            {
                if (metodo.cambio)
                {
                    metodoPago = "Solicitado pago en efectivo. Has pedido cambio";
                }
                else
                {
                    metodoPago = "Solicitado pago en efectivo. No has pedido cambio";
                }
            }
            foreach(SandwichPedido s in p.sandwichesPedidos)
            {
                sandwichesPedidos.Add(new SandwichPedidoDetailsViewModel(s));
            }
            precioTotal = p.Preciototal;
        }
    }

    public class SandwichPedidoDetailsViewModel
    {
        public string nombre { get; set; }
        public IList<string> Ingredientes { get; set; }
        [DataType(DataType.Currency)]
        public double precio { get; set; }
        public int cantidad { get; set; }

        public SandwichPedidoDetailsViewModel(SandwichPedido s)
        {
            nombre = s.Sandwich.SandwichName;
            Ingredientes = new List<string>();
            foreach(IngredienteSandwich  isa in s.Sandwich.IngredienteSandwich)
            {
                Ingredientes.Add(isa.Ingrediente.Nombre+" ");
            }
            cantidad = s.Cantidad;
            if (s.Sandwich.OfertaSandwich != null)
            {
                double porc = 0;
                foreach (OfertaSandwich o in s.Sandwich.OfertaSandwich)
                {
                    if(o.Porcentaje > porc)
                    {
                        porc = o.Porcentaje;
                    }
                }
                precio = s.Sandwich.Precio - (s.Sandwich.Precio*porc/100);
            }
            else
            {
                precio = s.Sandwich.Precio;
            }
            

        }
    }

    public class MetodoPagoDetailsViewModel
    {
        public string Tipo { get; set; }
        public string Tarjeta { get; set; }
        public bool cambio { get; set; }

        public MetodoPagoDetailsViewModel(MetodoDePago p)
        {
            if(p is Tarjeta)
            {
                Tipo = "Tarjeta";
                Tarjeta t = p as Tarjeta;
                Tarjeta = t.Numero.ToString()[12..15];
            }
            else
            {
                Tipo = "Efectivo";
                Efectivo e = p as Efectivo;
                cambio = e.NecesitasCambio;
            }
        }
    }
}
