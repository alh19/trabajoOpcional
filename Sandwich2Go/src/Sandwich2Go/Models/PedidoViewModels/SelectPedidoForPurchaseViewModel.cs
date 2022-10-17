using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models.PedidoViewModels
{
    public class SelectPedidoForPurchaseViewModel
    {
        //Utilizado para iltrar el nombre del proveedor
        [Display(Name = "Proveedor")]
        public string nombreProveedor { get; set; }
    }
}
