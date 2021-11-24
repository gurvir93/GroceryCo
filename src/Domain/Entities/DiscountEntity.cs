using System;

namespace GroceryCo.Domain.Entities
{
    public class DiscountEntity
    {
        public int UPC { get; set; }
        public DiscountTypeIDs DiscountType { get; set; }
        public string DiscountTypeID
        {
            get 
            {
                return DiscountType.ToString();
            } 
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DiscountPercent { get; set; }

        // Used for buy x get y discount types.
        public int ItemsRequired { get; set; } = 0;
    }
}
