using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sandwich2Go.Models.PedidoProvViewModels

{
    public class PedidoProvDetailsViewModel
    {
        //Datos Proveedor
        
        [Display(Name = "CIF/DNI:")]
        public string Cif { get; set; }
        [Display(Name = "Nombre del Proveedor:")]
        public string NombreProveedor { get; set; }
        [Display(Name = "Dirección del Proveedor:")]
        public string DireccionProveedor { get; set; }


        //Datos Ingredientes
        [Display(Name = "Nombre del Ingrediente:")]
        public string NombreIngrediente { get; set; }
        [Display(Name = "Stock del Ingrediente:")]
        public int Stock{ get; set; }
        [Display(Name = "Cantidad solicitada:")]
        public int Cantidad { get; set; }
        [Display(Name = "Precio unitario del Ingrediente:")]
        public int PrecioUnitario { get; set; }
        //Datos Pedido
        [Display(Name = "Precio total del Pedido:")]
        public double PrecioTotal { get; set; }

        public IList<Ingrediente> Ingredientes { get; set; }
        public PedidoProvDetailsViewModel(IngrProv p)
        {
            Cif = p.Proveedor.Cif;
            NombreProveedor = p.Proveedor.Nombre;
            DireccionProveedor = p.Proveedor.Direccion;
            NombreIngrediente = p.Ingrediente.Nombre;
            Stock = p.Ingrediente.Stock;
            Cantidad = p.IngrPedProvs.FirstOrDefault().Cantidad;
            PrecioUnitario = p.Ingrediente.PrecioUnitario;

        }

        public override bool Equals(object obj)
        {
            return obj is PedidoProvDetailsViewModel model &&
                this.Cif == model.Cif &&
                this.NombreProveedor == model.NombreProveedor &&
                this.DireccionProveedor == model.DireccionProveedor &&
                this.NombreIngrediente == model.NombreIngrediente &&
                this.Stock == model.Stock &&
                this.Cantidad == model.Cantidad &&
                this.PrecioUnitario == model.PrecioUnitario &&
                this.PrecioTotal == model.PrecioTotal;
        }
    }
}
