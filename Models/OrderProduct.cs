namespace MercerStore.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }

        public int OrderProductListId { get; set; }
        public OrderProductList OrderProductList { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}