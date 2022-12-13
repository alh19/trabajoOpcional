using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace Sandwich2Go.Models.PedidoSandwichPersonalizadoViewModels
{
    public class IngredientePedidoViewModel
    {
        public IngredientePedidoViewModel() {
            cantidad = 1;
        }

        public IngredientePedidoViewModel(Ingrediente ing)
        {
            this.Id = ing.Id;
            this.Nombre = ing.Nombre;
            this.PrecioUnitario = ing.PrecioUnitario;
            this.Alergenos = new List<string>();
            this.Stock = ing.Stock;
            foreach (AlergSandw ingSand in ing.AlergSandws)
            {
                //this.Ingredientes.Add(ingSand.Ingrediente.Nombre + " ");
                foreach (AlergSandw alSand in ingSand.Ingrediente.AlergSandws)
                {
                    if (!this.Alergenos.Contains(alSand.Alergeno.Name + " "))
                    {
                        this.Alergenos.Add(alSand.Alergeno.Name + " ");
                    }
                }
            }
            

        }
        [DataType(DataType.Currency)]
        
        public  int Id { get; set; }

        
        public  string Nombre
        {
            get; set;
        }
        [DataType(DataType.Currency)]
        public  double PrecioUnitario
        {
            get; set;
        }


        [Range(1, 100, ErrorMessage = "Introduce al menos uno")]
        public  int cantidad { get; set; }

        
        public  int Stock { get; set; }
        public  IList<string> Alergenos
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            return obj is IngredientePedidoViewModel model &&
                this.Id == model.Id &&
                this.Nombre == model.Nombre &&
                this.Stock==model.Stock &&
                this.PrecioUnitario == model.PrecioUnitario &&
                this.Alergenos.SequenceEqual(model.Alergenos);
               
        }
    }
}
