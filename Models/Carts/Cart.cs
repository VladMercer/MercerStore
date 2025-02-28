using MercerStore.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace MercerStore.Models.Carts
{
    public class Cart : IEntity
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }

        public List<CartProduct> CartProducts { get; set; }

    }
}
