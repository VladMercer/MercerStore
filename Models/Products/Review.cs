using MercerStore.Interfaces;
using MercerStore.Models.Users;
using MercerStore.Models.Products;

namespace MercerStore.Models.Products
{
    public class Review : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public bool Edited { get; set; } = false;
        public DateTime EditDateTime { get; set; }
        public int Value { get; set; }
        public string? ReviewText { get; set; }

    }
}
