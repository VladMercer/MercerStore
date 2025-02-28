using MercerStore.Interfaces;
using MercerStore.Models.Products;
namespace MercerStore.Models.Carts
{
    public class CartProduct : IEntity
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
