using MediatR;
using MercerStore.Web.Application.Interfaces;

namespace MercerStore.Web.Application.Handlers.Users.Queries
{
    public record GetCurrentUserRolesQuery() : IRequest<IEnumerable<string>>;
    public class GetCurrentUserRolesHandler : IRequestHandler<GetCurrentUserRolesQuery, IEnumerable<string>>
    {
        private readonly IUserIdentifierService _userIdentifierService;

        public GetCurrentUserRolesHandler(IUserIdentifierService userIdentifierService)
        {
            _userIdentifierService = userIdentifierService;
        }

        public Task<IEnumerable<string>> Handle(GetCurrentUserRolesQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_userIdentifierService.GetCurrentUserRoles());
        }
    }
}
