using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sandwich2Go.Models
{
    public class Ingrediente
    {

        //public Ingrediente()
        //{

        //}

        //public Ingrediente(int id, string nombre, int precioUnitario, int stock, IList<AlergSandw> alergSandws, IList<IngredienteSandwich> ingredienteSandwich, IList<IngrProv> ingrProv)
        //{
        //    Id = id;
        //    Nombre = nombre;
        //    PrecioUnitario = precioUnitario;
        //    Stock = stock;
        //    AlergSandws = alergSandws;
        //    IngredienteSandwich = ingredienteSandwich;
        //    IngrProv = ingrProv;
        //}

        //public Ingrediente(int id, string nombre, int precioUnitario, int stock, IList<AlergSandw> alergSandws)
        //{
        //    Id = id;
        //    Nombre = nombre;
        //    PrecioUnitario = precioUnitario;
        //    Stock = stock;
        //    AlergSandws = alergSandws;
        //}

        [Key]
        public virtual int Id { get; set; }


        [Required, StringLength(20, ErrorMessage = "First name cannot be longer than 20 characters.")]
        public virtual string Nombre { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio unitario")]
        public virtual int PrecioUnitario
        {
            get;
            set;
        }

        [Required, Display(Name = "stock")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public virtual int Stock { get; set; }
        public virtual IList<AlergSandw> AlergSandws { get; set; }

        public virtual IList<IngredienteSandwich> IngredienteSandwich
        {
            get;
            set;
        }

        public virtual IList<IngrProv> IngrProv { get; set; }

    }
}
