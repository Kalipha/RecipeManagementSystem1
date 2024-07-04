using RecipeManagementSystem.Data;
using System.ComponentModel.DataAnnotations;

namespace RecipeManagementSystem.Models.Recipe
{
    public class RecipeViewModel
    {


        [Key]
        public int Id { get; set; }

        [Display(Name = "Recipe Name")]
        [Required]
        public string RecipeName { get; set; }

        [Display(Name = "Recipe Description")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Ingredients")]
        [Required]
        public string Ingredients { get; set; } = default!;


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; } = default!;

        [Display(Name = "Procedure")]
        [Required]
        public string Procedure { get; set; }

        [Display(Name = "Image")]
        public IFormFile Image { get; set; }

        public string? Imagepath { get; set; }
    }
}
