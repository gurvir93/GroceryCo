using GroceryCo.Domain.Entities;
using System;

namespace GroceryCo.Domain.Interfaces
{
    public interface IDiscountManager
    {
        void CreateNewDiscount(string upc, DiscountTypeIDs discountType, int percentage, int? itemCount, DateTime startDate, DateTime endDate);
    }
}
