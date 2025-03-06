using MercerStore.Data.Enum.Review;
using MercerStore.Data.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MercerStore.Interfaces;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Repository;
using MercerStore.Services;
using Nest;
using System.Text.Json;
using MercerStore.Dtos.ResultDto;
using MercerStore.Dtos.ReviewDto;
using MercerStore.Dtos.SupplierDto;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/suppliers")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly IRequestContextService _requestContextService;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IRedisCacheService _redisCacheService;

        public SuppliersController(ISupplierRepository supplierRepository, IRequestContextService requestContextService, IRedisCacheService redisCacheService)
        {
            _supplierRepository = supplierRepository;
            _requestContextService = requestContextService;
            _redisCacheService = redisCacheService;
        }

        [HttpGet("suppliers/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetFilteredSuppliers(int pageNumber, int pageSize, string? query)
        {
            bool isDefaultQuery =
             pageNumber == 1 &&
             pageSize == 30 &&
             query == "";

            string cacheKey = $"suppliers:page1";
            if (isDefaultQuery)
            {
                var cachedData = await _redisCacheService.GetCacheAsync<string>(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    return Ok(JsonSerializer.Deserialize<object>(cachedData));
                }
            }
          
            var (supplierDtos, totalSuppliers) = await _supplierRepository.GetFilteredSuppliers(pageNumber, pageSize, query);

            var result = new PaginatedResultDto<AdminSupplierDto>(supplierDtos, totalSuppliers, pageSize);

            if (isDefaultQuery)
            {
                await _redisCacheService.SetCacheAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            }

            return Ok(result);
        }

        [HttpDelete("supplier/{supplierId}")]
        [LogUserAction("Manager remove supplier", "supplier")]
        public async Task<IActionResult> RemoveSupplier(int supplierId)
        {
            await _supplierRepository.RemoveSupplier(supplierId);

            var logDetails = new
            {
                supplierId
            };

            _requestContextService.SetLogDetails(logDetails);

            return Ok(supplierId);
        }
    }
}
