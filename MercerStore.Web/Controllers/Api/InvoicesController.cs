using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Invoices;

namespace MercerStore.Web.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/invoices")]
    [ApiController]

    public class InvoicesController : ControllerBase
    {
      
       private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("invoices")]
        public async Task<IActionResult> GetFilteredinvoices([FromQuery] InvoiceFilterRequest request)
        {
            var result = await _invoiceService.GetFilteredInvoices(request);
            return Ok(result);
        }
    }
}
