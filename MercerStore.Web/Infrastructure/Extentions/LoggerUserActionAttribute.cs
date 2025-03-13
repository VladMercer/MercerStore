using MercerStore.Web.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MercerStore.Web.Infrastructure.Extentions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogUserActionAttribute : ActionFilterAttribute
    {
        public string ActionName { get; }
        public string EntityName { get; }


        public LogUserActionAttribute(string actionName, string entityName)
        {
            ActionName = actionName;
            EntityName = entityName;

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var requestContextService = context.HttpContext.RequestServices.GetService<IRequestContextService>();
            var logDetails = requestContextService?.GetLogDetails();

            int? entityId = null;

           /* if (context.Result is ObjectResult result)
            {
                if (result.Value is IEntity entity)
                {
                    entityId = entity.Id;
                }
                if (result.Value is int id)
                {
                    entityId = id;
                }
            }*/

            if (context.Result is RedirectToActionResult redirectResult && redirectResult.RouteValues != null)
            {
                if (redirectResult.RouteValues.TryGetValue("id", out var idValue) && int.TryParse(idValue?.ToString(), out var id))
                {
                    entityId = id;
                }
            }

            var userIdentifierService = context.HttpContext.RequestServices.GetService<IUserIdentifierService>();
            var logService = context.HttpContext.RequestServices.GetService<ILogService>();

            var userId = userIdentifierService.GetCurrentIdentifier();
            var roles = userIdentifierService.GetCurrentUserRoles();

            logService?.LogUserAction(
                roles: roles,
                userId: userId,
                action: ActionName,
                entityName: EntityName,
                entityId: entityId ?? 0,
                details: logDetails
            );
        }
    }
}