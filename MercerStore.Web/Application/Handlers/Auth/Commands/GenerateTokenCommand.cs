using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Auth.Commands
{
    public record GenerateTokenCommand() : LoggableRequest<string>("Generate guest token", "Token");

    public class GenerateTokenHandler : IRequestHandler<GenerateTokenCommand, string>
    {
        private readonly IAuthService _authService;

        public GenerateTokenHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            var (token, guestId) = await _authService.GenerateGuestToken();

            request.EntityId = guestId;
            request.Details = new { guestId, token };

            return token;
        }
    }
}
