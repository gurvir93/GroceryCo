using GroceryCo.Infrastructure.Database;
using static GroceryCo.Infrastructure.Database.GroceryCoDatabase;
using System.Data;

namespace GroceryCo.Domain
{
    public class Database : IDatabase
    {
        public DataTable GetDiscountTable()
        {
            return new DiscountDataTable();
        }

        public DataTable GetDiscountTypeTable()
        {
            return new DiscountTypeDataTable();
        }

        public DataTable GetProductTable()
        {
            return new ProductDataTable();
        }

        public DataTable GetProductTypeTable()
        {
            return new ProductTypeDataTable();
        }
    }
}
