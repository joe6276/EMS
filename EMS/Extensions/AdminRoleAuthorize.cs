using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EMS.Extensions
{
    public class AdminRoleAuthorize : ActionFilterAttribute
    {
        // Override the OnActionExecuting method to check the user's role
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            // Check if the user is authenticated and has the Admin role
            if (user == null || !user.IsInRole("Admin"))
            {
                // Redirect to Access Denied page if the user doesn't have the Admin role
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }

            base.OnActionExecuting(context);
        }

    }

}
