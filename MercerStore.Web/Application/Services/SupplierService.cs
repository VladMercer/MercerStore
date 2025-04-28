using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Dtos.Supplier;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Invoices;
using MercerStore.Web.Application.Requests.Suppliers;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task RemoveSupplier(int supplierId, CancellationToken ct)
    {
        await _supplierRepository.RemoveSupplier(supplierId, ct);
    }

    public async Task<int> CreateSupplier(CreateSupplierViewModel createSupplierViewModel, CancellationToken ct)
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

        var newSupplier = await _supplierRepository.AddSupplier(supplier, ct);

        return newSupplier.Id;
    }

    public async Task<UpdateSupplierViewModel> GetUpdateSupplierViewModel(int supplierId, CancellationToken ct)
    {
        var supplier = await _supplierRepository.GetSupplierById(supplierId, ct);

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

    public async Task<Result<int>> UpdateSupplier(UpdateSupplierViewModel updateSupplierViewModel, CancellationToken ct)
    {
        var supplier = await _supplierRepository.GetSupplierById(updateSupplierViewModel.Id, ct);
        if (supplier == null) return Result<int>.Failure("Поставщик не найден");

        supplier.Address = updateSupplierViewModel.Address;
        supplier.Name = updateSupplierViewModel.Name;
        supplier.Phone = updateSupplierViewModel.Phone;
        supplier.Email = updateSupplierViewModel.Email;
        supplier.ContactPerson = updateSupplierViewModel.ContactPerson;
        supplier.IsCompany = updateSupplierViewModel.IsCompany;
        supplier.TaxId = updateSupplierViewModel.TaxId;

        var supplierId = await _supplierRepository.UpdateSupplier(supplier, ct);

        return Result<int>.Success(supplierId);
    }

    public async Task<PaginatedResultDto<AdminSupplierDto>> GetFilteredSuppliersWithoutCache(
        SupplierFilterRequest request, CancellationToken ct)
    {
        var (suppliers, totalSuppliers) = await _supplierRepository.GetFilteredSuppliers(request, ct);
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
