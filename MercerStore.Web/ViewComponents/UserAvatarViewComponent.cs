using MercerStore.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Web.ViewComponents;

public class UserAvatarViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAvatarViewComponent(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IViewComponentResult Invoke()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var profilePictureUrl = user.GetProfilePictureUrl();

        return View("Default", profilePictureUrl);
    }
}