using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IngrPedProv = Sandwich2Go.Models.IngrPedProv;
using System.Linq;

namespace Sandwich2Go.Models.PedidoProvViewModels
{
    public class IngrPedProvViewModel
    {
        public IngrPedProvViewModel() { }

        public IngrPedProvViewModel(IngrPedProv p)
        {
            this.Id = p.Id;
            this.Cantidad = p.Cantidad;
        }

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
        public virtual int PedidoId { get; set; }
        public virtual IList<int> PedidoProveedorId { get; set; }
        public virtual IList<int> IngrProvId { get; set; }
        public virtual IList<string> Alergenos { get; set; }
        public virtual IList<string> Ingredientes
        {
            get; set;
        }

        public virtual string Alm { get; set; }
        public virtual string IngM { get; set; }

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
                this.Alm == model.Alm &&
                this.IngM == model.IngM;
        }
    }
}
