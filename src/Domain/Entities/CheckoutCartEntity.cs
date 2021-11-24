using System.Collections.Generic;
using System.Linq;

namespace GroceryCo.Domain.Entities
{
    public class CheckoutCart
    {
        public List<CartEntity> Groceries;
        public decimal TotalPrice
        {
            get
            {
                return Groceries.Select(g => g.Price).Sum();
            }
        }

        public decimal TotalSaved
        {
            get
            {
                return Groceries.Select(g => g.PriceOff).Sum();
            }
        }

        public CheckoutCart()
        {
            Groceries = new List<CartEntity>();
        }
    }

    public class CartEntity
    {
        public int UPC { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal PriceOff { get; set; } = 0;
    }
}