using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Application.Dtos.UserDto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }

        [ModelBinder(BinderType = typeof(DateTimeModelBinder))]
        public DateTime CreateDate { get; set; }

        [ModelBinder(BinderType = typeof(DateTimeModelBinder))]
        public DateTime? LastActivityDate { get; set; }
        public string? PhoneNumber { get; set; }
        public int? CountReviews { get; set; }
        public int? CountOrders { get; set; }
        public List<string> Roles { get; set; }
    }
}
