using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace RecipeManagementSystem.ActionFilter
{
    public class RedirectAuthenticatedUserAttribute
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity!.IsAuthenticated)
            {
<<<<<<< HEAD
                context.Result = new RedirectToActionResult("Index", "Recipe", null);
=======
                context.Result = new RedirectToActionResult("Index", "recipe", null);
>>>>>>> 5fa03cbbb55b4349f355f97c3aff9bfa0c4b38f8
            }
        }
    }
}
