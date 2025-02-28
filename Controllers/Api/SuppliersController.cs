using MercerStore.Data.Enum.Review;
using MercerStore.Data.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MercerStore.Interfaces;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Repository;
using MercerStore.Services;

namespace MercerStore.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/suppliers")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly IRequestContextService _requestContextService;
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersController(ISupplierRepository supplierRepository, IRequestContextService requestContextService)
        {
            _supplierRepository = supplierRepository;
            _requestContextService = requestContextService;
        }

        [HttpGet("suppliers/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetFilteredSuppliers(int pageNumber, int pageSize, string? query)
        {

            var(supplierDtos, totalSuppliers) = await _supplierRepository.GetFilteredSuppliers(pageNumber, pageSize, query);

            var result = new
            {
                Suppliers = supplierDtos,
                TotalItems = totalSuppliers,
                TotalPages = (int)Math.Ceiling((double)totalSuppliers / pageSize)
            };

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
