using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.InvoiceDto;
using MercerStore.Web.Infrastructure.Data.Enum.Invoice;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Application.Models.Invoice;
using MercerStore.Web.Application.Models.Products;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IUserIdentifierService _userIdentifierService;
        private readonly ILogService _logService;
        private readonly IRequestContextService _requestContextService;
        private readonly IProductRepository _productRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository, IRedisCacheService redisCacheService, IUserIdentifierService userIdentifierService, ILogService logService, IRequestContextService requestContextService, IProductRepository productRepository)
        {
            _invoiceRepository = invoiceRepository;
            _redisCacheService = redisCacheService;
            _userIdentifierService = userIdentifierService;
            _logService = logService;
            _requestContextService = requestContextService;
            _productRepository = productRepository;
        }
        public async Task<PaginatedResultDto<InvoiceDto>> GetFilteredInvoices(InvoiceFilterRequest request)
        {

            bool isDefaultQuery =
            request.PageNumber == 1 &&
            request.PageSize == 30 &&
            !request.SortOrder.HasValue &&
            !request.Filter.HasValue &&
            request.Query == "";

            string cacheKey = $"invoices:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => FetchFilteredInvoicesAsync(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
        }

        private async Task<PaginatedResultDto<InvoiceDto>> FetchFilteredInvoicesAsync(InvoiceFilterRequest request)
        {
            var (invoices, totalItems) = await _invoiceRepository.GetFilteredInvoices(request);

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
                Phone = i.Supplier.Phone,

            }).ToList();

            return new PaginatedResultDto<InvoiceDto>(invoiceDtos, totalItems, request.PageSize);
        }
        public async Task<CreateInvoiceViewModel> AddInvoice(int supplierId)
        {
            var managerId = _userIdentifierService.GetCurrentIdentifier();

            var invoice = await _invoiceRepository.GetInvoiceByManagerId(managerId);


            if (invoice == null)
            {
                invoice = new Invoice
                {
                    ManagerId = managerId,
                    SupplierId = supplierId,
                    Status = InvoiceStatus.activ,
                };
                await _invoiceRepository.AddInvoice(invoice);
            }
            var availableProducts = await _productRepository.GetIsUnassignedProducts();

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
        public async Task<Result<AddInvoiceItemResultViewModel>> AddInvoiceItem(CreateInvoiceViewModel createInvoiceViewModel)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(createInvoiceViewModel.InvoiceId);

            if (invoice == null || invoice.Status == InvoiceStatus.Closed)
            {
                return Result<AddInvoiceItemResultViewModel>.Failure("Поставка не найдена");
            }

            Product? product = null;

            if (createInvoiceViewModel.ProductId.HasValue)
            {
                product = await _productRepository.GetProductByIdAsync(createInvoiceViewModel.ProductId);
            }

            else if (!string.IsNullOrEmpty(createInvoiceViewModel.Sku))
            {
                product = await _productRepository.GetProductBySku(createInvoiceViewModel.Sku);

            }

            if (product == null)
            {
                return Result<AddInvoiceItemResultViewModel>.Failure("Продукт не найден");
            }

            var newInvoiceItem = new InvoiceItem
            {
                InvoiceId = invoice.Id,
                ProductId = product.Id,
                Quantity = createInvoiceViewModel.Quantity,
                PurchasePrice = createInvoiceViewModel.ProductPrice,

            };

            await _invoiceRepository.AddInvoiceItem(newInvoiceItem);

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

            if (invoiceItems.Any())
            {
                await _invoiceRepository.AddInvoiceItems(invoiceItems);
            }

            var logDetails = new
            {
                createInvoiceViewModel.InvoiceId,
                ProductId = createInvoiceViewModel.ProductId ?? product.Id,
                createInvoiceViewModel.Quantity,
                PurchasePrice = createInvoiceViewModel.ProductPrice,
                createInvoiceViewModel.AvailableProducts,
                createInvoiceViewModel.Notes

            };

            _requestContextService.SetLogDetails(logDetails);

            var result = new AddInvoiceItemResultViewModel
            {
                ProductId = product.Id,
                SupplierId = invoice.SupplierId
            };
           
            return Result<AddInvoiceItemResultViewModel>.Success(result);
        }

        public async Task<Result<int>> CloseInvoice(int invoiceId, string notes)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);

            if (invoice == null || invoice.Status == InvoiceStatus.Closed)
            {
                return Result<int>.Failure("Поставка не найдена");
            }

            invoice.Notes = notes;

            foreach (var item in invoice.InvoiceItems)
            {
                await _productRepository.DecreaseProductStock(item.ProductId, item.Product.SKU, item.Quantity);
            }

            invoice.Status = InvoiceStatus.Received;

            invoice.TotalAmount = invoice.InvoiceItems.Sum(item => item.PurchasePrice * item.Quantity);
            await _invoiceRepository.UpdateInvoice(invoice);

            var logDetails = new
            {
                invoice.Id,
                invoice.ManagerId,
                invoice.SupplierId,
                invoice.TotalAmount,
                invoice.Notes
            };

            _requestContextService.SetLogDetails(logDetails);

            return Result<int>.Success(invoiceId);
        }
        public async Task<UpdateInvoiceViewModel> GetUpdateInvoiceViewModel(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);
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
        public async Task<int> UpdateInvoice(UpdateInvoiceViewModel updateInvoiceViewModel)
        {
            var updatedTotalAmount = updateInvoiceViewModel.InvoiceItems.Sum(i => i.Quantity * i.PurchasePrice);

            if (updateInvoiceViewModel.Status == InvoiceStatus.Closed)
            {
                updateInvoiceViewModel.PaymentDate = DateTime.UtcNow;
            }

            updateInvoiceViewModel.EditDate = DateTime.UtcNow;

            var invoice = await _invoiceRepository.GetInvoiceById(updateInvoiceViewModel.Id);

            invoice.EditDate = updateInvoiceViewModel.EditDate;
            invoice.PaymentDate = updateInvoiceViewModel.PaymentDate;
            invoice.Status = updateInvoiceViewModel.Status;
            invoice.Notes = updateInvoiceViewModel.Notes;
            invoice.TotalAmount = updatedTotalAmount;

            await _invoiceRepository.UpdateInvoice(invoice);

            var updateInvoiceItems = updateInvoiceViewModel.InvoiceItems.Select(i => new InvoiceItemViewModel
            {
                Id = i.Id,
                Quantity = i.Quantity,
                PurchasePrice = i.PurchasePrice
            }).ToList();

            await _invoiceRepository.UpdateInvoiceItems(updateInvoiceItems);

            var logDetails = new
            {
                updateInvoiceViewModel.Status,
                invoice.ManagerId,
                invoice.SupplierId
            };

            _requestContextService.SetLogDetails(logDetails);
            return invoice.Id;
        }
    }   
}
