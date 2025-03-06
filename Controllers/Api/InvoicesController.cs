using MercerStore.Data.Enum.Review;
using MercerStore.Data.Enum;
using Microsoft.AspNetCore.Mvc;
using MercerStore.Interfaces;
using MercerStore.Data.Enum.Invoice;
using Microsoft.AspNetCore.Authorization;
using MercerStore.Models.Products;
using MercerStore.Services;
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Printing;
using System.Text.Json;
using MercerStore.Dtos.ProductDto;
using MercerStore.Dtos.ResultDto;
using MercerStore.Dtos.InvoiceDto;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/invoices")]
    [ApiController]

    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IRedisCacheService _redisCacheService;
        public InvoicesController(IInvoiceRepository invoiceRepository, IRedisCacheService redisCacheService)
        {
            _invoiceRepository = invoiceRepository;
            _redisCacheService = redisCacheService;
        }

        [HttpGet("invoices/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetFilteredinvoices(int pageNumber, int pageSize, InvoiceSortOrder? sortOrder, TimePeriod? period, InvoiceFilter? filter, string? query)
        {
            bool isDefaultQuery =
            pageNumber == 1 &&
            pageSize == 30 &&
            !sortOrder.HasValue &&
            !filter.HasValue &&
            query == "";
            
            string cacheKey = $"invoices:page1";
            if (isDefaultQuery)
            {
                var cachedData = await _redisCacheService.GetCacheAsync<string>(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    return Ok(JsonSerializer.Deserialize<object>(cachedData));
                }
            }
          

            var (invoiceDtos, totalInvoices) = await _invoiceRepository.GetFilteredInvoices(pageNumber, pageSize, sortOrder, period, filter, query);

            var result = new PaginatedResultDto<InvoiceDto>(invoiceDtos, totalInvoices, pageSize);

            if (isDefaultQuery)
            {
                await _redisCacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            }

            return Ok(result);
        }
    }
}
