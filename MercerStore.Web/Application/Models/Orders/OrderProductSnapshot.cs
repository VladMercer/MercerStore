namespace MercerStore.Web.Application.Models.Orders
{

    public class OrderProductSnapshot
    {
        public int Id { get; set; }
        public int OrderProductListId { get; set; }
        public OrderProductList OrderProductList { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal PriceAtOrder { get; set; }
        public string ProductImageUrl { get; set; }
        public int Quantity { get; set; }


    }
}
