using MediatR;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Users.Queries;

public record GetCurrentIdentifierQuery : IRequest<string>;

public class GetCurrentIdentifierHandler : IRequestHandler<GetCurrentIdentifierQuery, string>
{
    private readonly IUserIdentifierService _userIdentifierService;

    public GetCurrentIdentifierHandler(IUserIdentifierService userIdentifierService)
    {
        _userIdentifierService = userIdentifierService;
    }

    public Task<string> Handle(GetCurrentIdentifierQuery request, CancellationToken ct)
    {
        return Task.FromResult(_userIdentifierService.GetCurrentIdentifier());
    }
}