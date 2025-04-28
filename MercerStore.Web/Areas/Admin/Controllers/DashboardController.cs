using MediatR;
using MercerStore.Web.Application.Handlers.Dashboard.Queries;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
public class DashboardController : Controller
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var dashboardViewModel = await _mediator.Send(new GetDashboardViewMetricQuery(), ct);
        return View(dashboardViewModel);
    }
}