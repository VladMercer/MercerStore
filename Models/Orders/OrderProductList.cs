namespace MercerStore.Models.Orders
{
    public class OrderProductList
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<OrderProductSnapshot> OrderProductSnapshots { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
