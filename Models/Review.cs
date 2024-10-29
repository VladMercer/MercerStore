﻿using Microsoft.AspNet.Identity;

namespace MercerStore.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int Value { get; set; }
        public string? ReviewText { get; set; }
    }
}
