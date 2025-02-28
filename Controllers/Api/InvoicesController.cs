using MercerStore.Data.Enum.Review;
using MercerStore.Data.Enum;
using Microsoft.AspNetCore.Mvc;
using MercerStore.Interfaces;
using MercerStore.Data.Enum.Invoice;
using Microsoft.AspNetCore.Authorization;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/invoices")]
    [ApiController]

    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoicesController(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        [HttpGet("invoices/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetFilteredinvoices(int pageNumber, int pageSize, InvoiceSortOrder? sortOrder, TimePeriod? period, InvoiceFilter? filter, string? query)
        {

            var (invoiceDtos, totalInvoices) = await _invoiceRepository.GetFilteredInvoices(pageNumber, pageSize, sortOrder, period, filter, query);

            var result = new
            {
                Invoices = invoiceDtos,
                TotalItems = totalInvoices,
                TotalPages = (int)Math.Ceiling((double)totalInvoices / pageSize)
            }; 

            return Ok(result);
        }
    }
}
