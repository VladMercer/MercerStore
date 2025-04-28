using MercerStore.Web.Application.Dtos.Invoice;
using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Invoices;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Infrastructure.Data.Enum.Invoice;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IProductRepository _productRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository, IProductRepository productRepository)
    {
        _invoiceRepository = invoiceRepository;
        _productRepository = productRepository;
    }

    public async Task<PaginatedResultDto<InvoiceDto>> GetFilteredInvoicesWithoutCache(InvoiceFilterRequest request,
        CancellationToken ct)
    {
        var (invoices, totalItems) = await _invoiceRepository.GetFilteredInvoices(request, ct);

        var invoiceDtos = invoices.Select(i => new InvoiceDto
        {
            Id = i.Id,
            ManagerId = i.ManagerId,
            SupplierId = i.SupplierId,
            Status = i.Status switch
            {
                InvoiceStatus.Pending => "Ожидается",
                InvoiceStatus.Received => "Получена",
                InvoiceStatus.PartiallyReceived => "Частично получена",
                InvoiceStatus.Rejected => "Отклонена",
                InvoiceStatus.Closed => "Закрыта",
                _ => "Неизвестный статус"
            },
            CompanyName = i.Supplier.Name,
            DateReceived = i.DateReceived,
            PaymentDate = i.PaymentDate,
            TotalAmount = i.TotalAmount,
            EditDate = i.EditDate,
            Email = i.Supplier.Email,
            Phone = i.Supplier.Phone
        }).ToList();

        return new PaginatedResultDto<InvoiceDto>(invoiceDtos, totalItems, request.PageSize);
    }

    public async Task<CreateInvoiceViewModel> GetCreateInvoiceViewModel(int supplierId, string managerId,
        CancellationToken ct)
    {
        var invoice = await _invoiceRepository.GetInvoiceByManagerId(managerId, ct);

        if (invoice == null)
        {
            invoice = new Invoice
            {
                ManagerId = managerId,
                SupplierId = supplierId,
                Status = InvoiceStatus.Active
            };
            await _invoiceRepository.AddInvoice(invoice, ct);
        }

        var availableProducts = await _productRepository.GetIsUnassignedProducts(ct);

        return new CreateInvoiceViewModel
        {
            InvoiceId = invoice.Id,
            SupplierId = invoice.SupplierId,
            AvailableProducts = availableProducts.Select(a => new InvoiceProductSelectionViewModel
            {
                ProductId = a.Id,
                ProductName = a.Name,
                Quantity = 1,
                PurchasePrice = 0
            }).ToList()
        };
    }

    public async Task<Result<Invoice>> AddInvoiceItem(CreateInvoiceViewModel createInvoiceViewModel,
        CancellationToken ct)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(createInvoiceViewModel.InvoiceId, ct);

        if (invoice == null || invoice.Status == InvoiceStatus.Closed)
            return Result<Invoice>.Failure("Поставка не найдена");

        Product? product = null;

        if (createInvoiceViewModel.ProductId.HasValue)
            product = await _productRepository.GetProductByIdAsync(createInvoiceViewModel.ProductId, ct);

        else if (!string.IsNullOrEmpty(createInvoiceViewModel.Sku))
            product = await _productRepository.GetProductBySku(createInvoiceViewModel.Sku, ct);

        if (product == null) return Result<Invoice>.Failure("Продукт не найден");

        var newInvoiceItem = new InvoiceItem
        {
            InvoiceId = invoice.Id,
            ProductId = product.Id,
            Quantity = createInvoiceViewModel.Quantity,
            PurchasePrice = createInvoiceViewModel.ProductPrice
        };

        await _invoiceRepository.AddInvoiceItem(newInvoiceItem, ct);

        var selectedProducts = createInvoiceViewModel.AvailableProducts?
            .Where(p => p.IsSelected)
            .ToList() ?? new List<InvoiceProductSelectionViewModel>();

        var invoiceItems = selectedProducts.Select(sp => new InvoiceItem
        {
            InvoiceId = invoice.Id,
            ProductId = sp.ProductId,
            Quantity = sp.Quantity,
            PurchasePrice = sp.PurchasePrice
        }).ToList();

        if (invoiceItems.Any()) await _invoiceRepository.AddInvoiceItems(invoiceItems, ct);

        return Result<Invoice>.Success(invoice);
    }

    public async Task<Result<Invoice>> CloseInvoice(int invoiceId, string notes, CancellationToken ct)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(invoiceId, ct);

        if (invoice == null || invoice.Status == InvoiceStatus.Closed)
            return Result<Invoice>.Failure("Поставка не найдена");

        invoice.Notes = notes;

        foreach (var item in invoice.InvoiceItems)
            await _productRepository.DecreaseProductStock(item.ProductId, item.Product.SKU, item.Quantity, ct);

        invoice.Status = InvoiceStatus.Received;

        invoice.TotalAmount = invoice.InvoiceItems.Sum(item => item.PurchasePrice * item.Quantity);
        await _invoiceRepository.UpdateInvoice(invoice, ct);

        return Result<Invoice>.Success(invoice);
    }

    public async Task<UpdateInvoiceViewModel> GetUpdateInvoiceViewModel(int invoiceId, CancellationToken ct)
    {
        var invoice = await _invoiceRepository.GetInvoiceById(invoiceId, ct);
        return new UpdateInvoiceViewModel
        {
            Id = invoice.Id,
            DateReceived = invoice.DateReceived,
            EditDate = invoice.EditDate,
            PaymentDate = invoice.PaymentDate,
            SupplierId = invoice.SupplierId,
            CompanyName = invoice.Supplier.Name,
            Status = invoice.Status,
            Notes = invoice.Notes,
            TotalAmount = invoice.TotalAmount,
            InvoiceItems = invoice.InvoiceItems.Select(i => new InvoiceItemViewModel
            {
                Id = i.Id,
                ImageUrl = i.Product.MainImageUrl,
                ProductId = i.Product.Id,
                ProductName = i.Product.Name,
                PurchasePrice = i.PurchasePrice,
                Quantity = i.Quantity
            }).ToList()
        };
    }

    public async Task<Invoice> UpdateInvoice(UpdateInvoiceViewModel updateInvoiceViewModel, CancellationToken ct)
    {
        var updatedTotalAmount = updateInvoiceViewModel.InvoiceItems.Sum(i => i.Quantity * i.PurchasePrice);

        if (updateInvoiceViewModel is { Status: InvoiceStatus.Closed, PaymentDate: null })
            updateInvoiceViewModel.PaymentDate = DateTime.UtcNow;

        updateInvoiceViewModel.EditDate = DateTime.UtcNow;

        var invoice = await _invoiceRepository.GetInvoiceById(updateInvoiceViewModel.Id, ct);

        invoice.EditDate = updateInvoiceViewModel.EditDate;
        invoice.PaymentDate = updateInvoiceViewModel.PaymentDate;
        invoice.Status = updateInvoiceViewModel.Status;
        invoice.Notes = updateInvoiceViewModel.Notes;
        invoice.TotalAmount = updatedTotalAmount;

        await _invoiceRepository.UpdateInvoice(invoice, ct);

        var updateInvoiceItems = updateInvoiceViewModel.InvoiceItems.Select(i => new InvoiceItemViewModel
        {
            Id = i.Id,
            Quantity = i.Quantity,
            PurchasePrice = i.PurchasePrice
        }).ToList();

        await _invoiceRepository.UpdateInvoiceItems(updateInvoiceItems, ct);

        return invoice;
    }
}