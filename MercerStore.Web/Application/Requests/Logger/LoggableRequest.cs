using MediatR;

namespace MercerStore.Web.Application.Requests.Log;

public abstract record LoggableRequest<TResponse>(string Action, string EntityName) : IRequest<TResponse>
{
    public object? EntityId { get; set; }
    public object? Details { get; set; }
}
