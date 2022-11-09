using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sandwich2Go.Models.SandwichViewModels
{
    public class SandwichForPurchaseViewModel
    {
        public SandwichForPurchaseViewModel()
        {

        }

        public SandwichForPurchaseViewModel(Sandwich sandwich)
        {
            this.SandwichID = sandwich.Id;
            this.SandwichName = sandwich.SandwichName;
            this.Precio = sandwich.Precio;
            this.ingredientes = new string[sandwich.IngredienteSandwich.Count];
            this.Desc = sandwich.Desc;
            this.alergenos = new string[0];
            int i = 0;
            foreach(IngredienteSandwich ing in sandwich.IngredienteSandwich)
            {
                this.ingredientes[i] = ing.Ingrediente.Nombre+" ";
                foreach(AlergSandw als in ing.Ingrediente.AlergSandws)
                {
                    if (!alergenos.Contains(als.Alergeno.Name)){
                        alergenos = alergenos.Concat(new string[] {(als.Alergeno.Name+" ")}).ToArray();
                    }
                }

                i++;
            }
        }

        [Key]
        public int SandwichID
        { get; set; }

        [Required]
        public string SandwichName { get; set; }


        [Required]
        [DataType(DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "Minimum price is 1 ")]
        [Display(Name = "Price For Purchase")]
        public virtual double Precio
        {
            get;
            set;
        }

        [Required]
        public string[] ingredientes { get; set; }

        public string[] alergenos { get; set; }

        public string Desc { get; set; }

        public override bool Equals(object obj)
        {

            return obj is SandwichForPurchaseViewModel sandwich &&
                this.SandwichID == sandwich.SandwichID &&
                this.SandwichName == sandwich.SandwichName &&
                this.Precio == sandwich.Precio &&
                this.Desc == sandwich.Desc &&
                this.ingredientes.SequenceEqual(sandwich.ingredientes) &&
                this.alergenos.SequenceEqual(sandwich.alergenos);
        }


    }
}
