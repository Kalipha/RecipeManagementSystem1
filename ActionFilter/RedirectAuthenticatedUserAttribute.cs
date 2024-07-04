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
                context.Result = new RedirectToActionResult("Index", "Recipe", null);
            }
        }
    }
}
