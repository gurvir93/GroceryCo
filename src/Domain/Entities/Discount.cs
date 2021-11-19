using System;

namespace GroceryCo.Domain.Entities
{
    public class Discount
    {
        public int UPC { get; set; }
        public DiscountTypes DiscountType { get; set; }
        public string DiscountTypeID
        {
            get 
            {
                return nameof(DiscountType);
            } 
        }
        public string DiscountTypeDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
