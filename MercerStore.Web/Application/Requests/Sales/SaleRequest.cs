namespace MercerStore.Web.Application.Requests.Sales;

public record SaleRequest(int SaleId, int? ProductId, string? Sku, int Quantity);