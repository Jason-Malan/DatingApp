using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
#nullable disable

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            var userDataManager = resultContext.HttpContext.RequestServices.GetService<IPlatformUserDataManager>();
            var user = await userDataManager.GetUserByIdAsync(userId);
            user.LastActive = DateTime.Now;
            await userDataManager.SaveAllAsync();
        }
    }
}
