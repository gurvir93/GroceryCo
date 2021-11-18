using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace GroceryCo.Domain.Entity
{
    public class GroceryList
    {
        public List<Products> Groceries { get; set; }
        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                total = Groceries.Where(g => !g.IsDiscounted).Sum(p => p.Price);
                total += Groceries.Where(g => g.IsDiscounted).Sum(p => Math.Round(p.Price * (p.DiscountPercent / 100), 2));

                return total;
            }
        }
    }
}
