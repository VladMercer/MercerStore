using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MercerStore.Web.Infrastructure.Extentions;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Suppliers;

namespace MercerStore.Web.Controllers.Api
{
    [Authorize(Roles = "Admin,Manager")]
    [Route("api/suppliers")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet("suppliers")]
        public async Task<IActionResult> GetFilteredSuppliers([FromQuery] SupplierFilterRequest request)
        {
            var result = await _supplierService.GetFilteredSuppliers(request);
            return Ok(result);
        }

        [HttpDelete("supplier/{supplierId}")]
        [LogUserAction("Manager remove supplier", "supplier")]
        public async Task<IActionResult> RemoveSupplier(int supplierId)
        {
            await _supplierService.RemoveSupplier(supplierId);
            return Ok(supplierId);
        }
    }
}
