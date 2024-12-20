using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using System.Security.Claims;

namespace MercerStore.Services
{
    public class UserIdentifierService : IUserIdentifierService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdentifierService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

		public string GetCurrentIdentifier()
		{
			var user = _httpContextAccessor.HttpContext?.User;
			var userId = user?.GetUserId();
			return userId;
		}
		public string GetCurrentUserRoles()
		{
			var user = _httpContextAccessor.HttpContext?.User;
			var roles = user?.GetUserRoles();
			return roles;
		}
	}
}
