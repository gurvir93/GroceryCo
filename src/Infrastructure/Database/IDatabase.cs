using System.Data;

namespace GroceryCo.Infrastructure.Database
{
    public interface IDatabase
    {
        DataTable GetDiscountTable();
        DataTable GetDiscountTypeTable();
        DataTable GetProductTable();
        DataTable GetProductTypeTable();
    }
}
