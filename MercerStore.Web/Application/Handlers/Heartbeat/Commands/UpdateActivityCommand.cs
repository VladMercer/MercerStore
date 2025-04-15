using MediatR;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Heartbeat.Commands
{
    public record UpdateActivityCommand() : IRequest<Unit>;
    public class HeartbeatHandler : IRequestHandler<UpdateActivityCommand, Unit>
    {
        private readonly IUserActivityService _userActivityService;

        public HeartbeatHandler(IUserActivityService userActivityService)
        {
            _userActivityService = userActivityService;
        }

        public async Task<Unit> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            await _userActivityService.UpdateUserActivity();
            return Unit.Value;
        }
    }
}
