using GroceryCo.Domain.Entities;
using System;

namespace GroceryCo.Domain.Interfaces
{
    public interface IDiscountManager
    {
        
        public void CreateNewDiscount(string upc, DiscountTypes discountType, int percentage, int? itemCount, DateTime startDate, DateTime endDate);

    }
}
