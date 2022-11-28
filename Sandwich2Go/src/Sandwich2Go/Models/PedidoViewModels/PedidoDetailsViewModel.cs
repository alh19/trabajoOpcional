using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models.PedidoViewModels
{
    public class PedidoDetailsViewModel
    {
        public string clienteNombre { get; set; }
        public string clienteApellidos { get; set; }
        public string fechaCompra { get; set; }
        public string direccionEntrega { get; set; }
        [DataType(DataType.Currency)]
        public double precioTotal { get; set; }
        public MetodoPagoDetailsViewModel metodoPago { get; set; }
        public IList<SandwichPedidoDetailsViewModel> sandwichesPedidos { get; set; }

        public PedidoDetailsViewModel(Pedido p)
        {
            clienteNombre = p.Cliente.Nombre;
            clienteApellidos = p.Cliente.Apellido;
            fechaCompra = p.Fecha.ToString();
            direccionEntrega = p.Direccion;
            precioTotal = p.Preciototal;
            sandwichesPedidos = new List<SandwichPedidoDetailsViewModel>();
            metodoPago = new MetodoPagoDetailsViewModel(p.MetodoDePago);
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
                Ingredientes.Add(isa.Ingrediente.Nombre);
            }
            cantidad = s.Cantidad;
            if (s.Sandwich.OfertaSandwich != null)
            {

                foreach (OfertaSandwich o in s.Sandwich.OfertaSandwich)
                {

                }
            }
            else
            {
                precio = s.Sandwich.Precio;
            }
            
            precio = ;

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
