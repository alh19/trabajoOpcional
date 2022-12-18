using Design;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sandwich2Go.Models.PedidoProvViewModels

{
    public class PedidoProvDetailsViewModel
    {
        //Datos Proveedor
        public int Id { get; set; }
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

        public IList<PedidoProv> PedidoProv { get; set; }
        public IList<Ingrediente> Ingredientes { get; set; }
        public IList<IngrProv> IngrProv { get; set; }
        public IList<IngrPedProv> IngrPedProv { get; set; }

        public PedidoProvDetailsViewModel() { }

        public PedidoProvDetailsViewModel(IngrProv p)
        {
            Id = p.Proveedor.Id;
            Cif = p.Proveedor.Cif;
            NombreProveedor = p.Proveedor.Nombre;
            DireccionProveedor = p.Proveedor.Direccion;
            NombreIngrediente = p.Ingrediente.Nombre;
            Stock = p.Ingrediente.Stock;
            Cantidad = p.IngrPedProvs.FirstOrDefault().Cantidad;
            PrecioUnitario = p.Ingrediente.PrecioUnitario;

        }

        public PedidoProvDetailsViewModel(IngrPedProv p)
        {
            Id = p.IngrProv.Proveedor.Id;
            Cif = p.IngrProv.Proveedor.Cif;
            NombreProveedor = p.IngrProv.Proveedor.Nombre;
            DireccionProveedor = p.IngrProv.Proveedor.Direccion;
            NombreIngrediente = p.IngrProv.Ingrediente.Nombre;
            Stock = p.IngrProv.Ingrediente.Stock;
            Cantidad = p.Cantidad;
            PrecioUnitario = p.IngrProv.Ingrediente.PrecioUnitario;
            //IngrPedProv = p;

        }

        public PedidoProvDetailsViewModel(PedidoProv p)
        {
            Id = p.IngrPedProv.First().IngrProv.Proveedor.Id;
            Cif = p.IngrPedProv.First().IngrProv.Proveedor.Cif;
            NombreProveedor = p.IngrPedProv.First().IngrProv.Proveedor.Nombre;
            DireccionProveedor = p.IngrPedProv.First().IngrProv.Proveedor.Direccion;
            NombreIngrediente = p.IngrPedProv.First().IngrProv.Ingrediente.Nombre;
            Stock = p.IngrPedProv.First().IngrProv.Ingrediente.Stock;
            Cantidad = p.IngrPedProv.First().Cantidad;
            PrecioUnitario = p.IngrPedProv.First().IngrProv.Ingrediente.PrecioUnitario;
            //IngrPedProv = new List<IngrPedProv>();
            //foreach (IngrPedProv s in p.IngrPedProv)
            //{
            //    IngrPedProv.Add(new IngrPedProv(
            //        s.Id,
            //        s.Cantidad,
            //        s.PedidoProv,
            //        s.PedidoProvId,
            //        s.IngrProv,
            //        s.IngrProvId
            //    ));
            //}
            IngrPedProv = (IList<IngrPedProv>)p.IngrPedProv
                .Select(x => new IngrPedProvDetailsViewModel(x)).ToList();
        }

        //public override bool Equals(object obj)
        //{
        //    return obj is PedidoProvDetailsViewModel model &&
        //        this.Id == model.Id &&
        //        this.Cif == model.Cif &&
        //        this.NombreProveedor == model.NombreProveedor &&
        //        this.DireccionProveedor == model.DireccionProveedor &&
        //        this.NombreIngrediente == model.NombreIngrediente &&
        //        this.Stock == model.Stock &&
        //        this.Cantidad == model.Cantidad &&
        //        this.PrecioUnitario == model.PrecioUnitario &&
        //        this.PrecioTotal == model.PrecioTotal &&
        //        this.PedidoProv.SequenceEqual(model.PedidoProv) &&
        //        this.IngrProv.SequenceEqual(model.IngrProv) &&
        //        this.IngrPedProv.SequenceEqual(model.IngrPedProv) &&
        //        this.Ingredientes.SequenceEqual(model.Ingredientes);
        //}

        public override bool Equals(object obj)
        {
            var result = true;

            var model = obj as PedidoProvDetailsViewModel;
            result = result &&
                this.Id == model.Id &&
                this.Cif == model.Cif &&
                this.NombreProveedor == model.NombreProveedor &&
                this.DireccionProveedor == model.DireccionProveedor &&
                this.NombreIngrediente == model.NombreIngrediente &&
                this.Stock == model.Stock &&
                this.Cantidad == model.Cantidad &&
                this.PrecioUnitario == model.PrecioUnitario &&
                this.PrecioTotal == model.PrecioTotal &&
                this.PedidoProv.SequenceEqual(model.PedidoProv) &&
                this.IngrProv.SequenceEqual(model.IngrProv) &&
                this.IngrPedProv.SequenceEqual(model.IngrPedProv) &&
                this.Ingredientes.SequenceEqual(model.Ingredientes);

            return result;

        }
    }

    public class IngrPedProvDetailsViewModel
    {

        public IngrPedProvDetailsViewModel()
        {

        }
        public IngrPedProvDetailsViewModel(IngrProv ingrprov) : this()
        {
            Id = ingrprov.Id;
            foreach (IngrPedProv pp in ingrprov.IngrPedProvs)
            {
                Cantidad.Equals(pp.Cantidad);
                PedidoProv.Equals(pp.PedidoProv);
                IngrProvs.Add(pp.IngrProv);
            }
        }
        public IngrPedProvDetailsViewModel(IngrPedProv ingrpedprov) : this(ingrpedprov.IngrProv)
        {
            Id = ingrpedprov.Id;
            PedidoProv = ingrpedprov.PedidoProv;
            PedidoProvId = ingrpedprov.PedidoProvId;
            IngrProvId = ingrpedprov.IngrProvId;
            Cantidad = ingrpedprov.Cantidad;
            IngrProvs = new List<IngrProv>()
            {
                new IngrProv()
                {
                    Id = ingrpedprov.IngrProv.Id,
                    Ingrediente = ingrpedprov.IngrProv.Ingrediente,
                    IngredienteId = ingrpedprov.IngrProv.IngredienteId,
                    Proveedor = ingrpedprov.IngrProv.Proveedor,
                    ProveedorId = ingrpedprov.IngrProv.ProveedorId,
                    IngrPedProvs = (IList<IngrPedProv>)ingrpedprov
                    //IngrPedProvs = new IngrPedProv
                    //(
                    //    Id = ingrpedprov.Id,
                    //    Cantidad = ingrpedprov.Cantidad,
                    //    PedidoProv = ingrpedprov.PedidoProv,
                    //    PedidoProvId = ingrpedprov.PedidoProvId,
                    //    foreach(IngrProv ing in ingrpedprov.IngrProv)
                    //    {
                    //            IngrProv.
                    //    }
                    //    IngrProvId = ingrpedprov.IngrProvId
                    //)

                }
            };
        }

        public int Id { get; set; }
        public PedidoProv PedidoProv { get; set; }
        public int PedidoProvId { get; set; }
        public IList<IngrProv> IngrProvs { get; set; }
        public virtual int IngrProvId { get; set; }
        public virtual int Cantidad { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is IngrPedProvDetailsViewModel model &&
                   Id == model.Id &&
                   PedidoProv == model.PedidoProv &&
                   PedidoProvId == model.PedidoProvId &&
                   IngrProvId == model.IngrProvId &&
                   IngrProvs.SequenceEqual(model.IngrProvs) &&
                   Cantidad == model.Cantidad;
        }
    }
}
