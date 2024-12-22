namespace MercerStore.Models
{
    public class OrderProductList
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
