using GroceryCo.Infrastructure.Database;
using System.Data;

namespace GroceryCo.Domain
{
    public class GroceryCoDatabase
    {
        public DataTable ProductTable { get; set; }
        public DataTable ProductTypeTable { get; set; }
        public DataTable DiscountDataTable { get; set; }
        public DataTable DiscountTypeDataTable { get; set; }

        public GroceryCoDatabase(IDatabase db)
        {
            ProductTable = db.GetProductTable();
            ProductTypeTable = db.GetProductTypeTable();
            DiscountDataTable = db.GetDiscountTable();
            DiscountTypeDataTable = db.GetDiscountTypeTable();
        }
    }
}
