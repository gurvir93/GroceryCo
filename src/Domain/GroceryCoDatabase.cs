using GroceryCo.Infrastructure.Database;
using static GroceryCo.Infrastructure.Database.GroceryCoDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GroceryCo.Domain
{
    public class GroceryCoDatabase
    {
        public DataTable ProductTable { get; set; }
        public DataTable ProductTypeTable { get; set; }
        public DataTable DiscountDataTable { get; set; }
        public DataTable DiscountTypeDataTable { get; set; }

        public GroceryCoDatabase()
        {
            ProductTable = new ProductDataTable();
            ProductTypeTable = new ProductTypeDataTable();
            DiscountDataTable = new DiscountDataTable();
            DiscountTypeDataTable = new DiscountTypeDataTable();
        }
    }
}
