using System.Collections.Generic;

namespace GroceryCo.Domain.Entities
{
    public class CheckoutCart
    {
        public List<Product> Groceries { get; set; }
        public Dictionary<int, Discount> DiscountedProducts { get; set; }
        public decimal TotalPrice { get; }
    }
}