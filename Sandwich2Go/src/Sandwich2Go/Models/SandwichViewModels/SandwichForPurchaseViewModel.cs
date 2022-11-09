using System.ComponentModel.DataAnnotations;

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
            this.ingredientes = new int[sandwich.IngredienteSandwich.Count];
            this.Desc = sandwich.Desc;
            int i = 0;
            foreach(IngredienteSandwich ing in sandwich.IngredienteSandwich)
            {
                this.ingredientes[i] = ing.IngredienteId;
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
        public int[] ingredientes { get; set; }

        public string Desc { get; set; }


    }
}
