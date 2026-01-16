using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SSO.Client.VAMS.Filters
{
    /// Ensures user is logged in via SSO
    public class RequireSsoLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            if (string.IsNullOrEmpty(session.GetString("access_token")))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
