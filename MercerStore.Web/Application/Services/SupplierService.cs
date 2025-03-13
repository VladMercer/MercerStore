using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Requests.Suppliers;
using MercerStore.Web.Application.Dtos.SupplierDto;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Invoice;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IRequestContextService _requestContextService;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogService _logService;


        public SupplierService(
            IRequestContextService requestContextService,
            ISupplierRepository supplierRepository,
            IRedisCacheService redisCacheService,
            ILogService logService)
        {
            _requestContextService = requestContextService;
            _supplierRepository = supplierRepository;
            _redisCacheService = redisCacheService;
            _logService = logService;
        }
        public async Task<PaginatedResultDto<AdminSupplierDto>> GetFilteredSuppliers(SupplierFilterRequest request)
        {
            bool isDefaultQuery =
            request.PageNumber == 1 &&
            request.PageSize == 30 &&
            request.Query == "";

            string cacheKey = $"suppliers:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => FetchFilteredSuppliers(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }
        public async Task RemoveSupplier(int supplierId)
        {
            await _supplierRepository.RemoveSupplier(supplierId);

            var logDetails = new
            {
                supplierId
            };

            _requestContextService.SetLogDetails(logDetails);
        }
        public async Task<int> CreateSupplier(CreateSupplierViewModel createSupplierViewModel)
        {
            var supplier = new Supplier
            {
                Name = createSupplierViewModel.Name,
                Address = createSupplierViewModel.Address,
                Phone = createSupplierViewModel.Phone,
                ContactPerson = createSupplierViewModel.ContactPerson,
                Email = createSupplierViewModel.Email,
                IsCompany = createSupplierViewModel.IsCompany,
                TaxId = createSupplierViewModel.TaxId
            };

            var newSupplier = await _supplierRepository.AddSupplier(supplier);

            var logDetails = new
            {
                newSupplier.Id,
                createSupplierViewModel.Name,
                createSupplierViewModel.Address,
                createSupplierViewModel.Phone,
                createSupplierViewModel.ContactPerson,
                createSupplierViewModel.Email,
                createSupplierViewModel.IsCompany,
                createSupplierViewModel.TaxId

            };

            _requestContextService.SetLogDetails(logDetails);

            return newSupplier.Id;
        }
        public async Task<UpdateSupplierViewModel> GetUpdateSupplierViewModel(int supplierId)
        {
            var supplier = await _supplierRepository.GetSupplierById(supplierId);

            return new UpdateSupplierViewModel
            {
                Id = supplier.Id,
                Address = supplier.Address,
                Phone = supplier.Phone,
                Name = supplier.Name,
                ContactPerson = supplier.ContactPerson,
                Email = supplier.Email,
                IsCompany = supplier.IsCompany,
                TaxId = supplier.TaxId
            };
        }
        public async Task<Result<int>> UpdateSupplier(UpdateSupplierViewModel updateSupplierViewModel)
        {
            var supplier = await _supplierRepository.GetSupplierById(updateSupplierViewModel.Id);
            if (supplier == null)
            {
                return Result<int>.Failure("Поставщик не найден");
            }

            supplier.Address = updateSupplierViewModel.Address;
            supplier.Name = updateSupplierViewModel.Name;
            supplier.Phone = updateSupplierViewModel.Phone;
            supplier.Email = updateSupplierViewModel.Email;
            supplier.ContactPerson = updateSupplierViewModel.ContactPerson;
            supplier.IsCompany = updateSupplierViewModel.IsCompany;
            supplier.TaxId = updateSupplierViewModel.TaxId;

            var logDetails = new
            {
                supplier.Id,
                supplier.Address,
                supplier.Name,
                supplier.Phone,
                supplier.Email,
                supplier.ContactPerson,
                supplier.IsCompany,
                supplier.TaxId
            };

            _requestContextService.SetLogDetails(logDetails);

            var supplierId = await _supplierRepository.UpdateSupplier(supplier);

            return Result<int>.Success(supplierId);
        }
        private async Task<PaginatedResultDto<AdminSupplierDto>> FetchFilteredSuppliers(SupplierFilterRequest request)
        {
            var (suppliers, totalSuppliers) = await _supplierRepository.GetFilteredSuppliers(request);
            var supplierDtos = suppliers.Select(s => new AdminSupplierDto
            {
                Id = s.Id,
                Phone = s.Phone,
                Address = s.Address,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                IsCompany = s.IsCompany,
                Name = s.Name,
                TaxId = s.TaxId
            }).ToList();

            var result = new PaginatedResultDto<AdminSupplierDto>(supplierDtos, totalSuppliers, request.PageSize);
         
            return result;
        }
    }
}
