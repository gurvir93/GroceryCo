using System;

namespace GroceryCo.Domain
{
    public class Products
    {
        public string Name { get; set; }
        public string UPC { get; set; }
        public decimal Price { get; set; } = 0.0m;
        public bool IsDiscounted { get; set; } = false;
        public int DiscountPercent { get; set; } = 0;
    }
}
