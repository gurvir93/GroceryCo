using GroceryCo.Infrastructure.Database;
using GroceryCo.Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace GroceryCo.Domain
{
    public class ProductTypeLoader
    {
        private DataTable _productTypesTable;
        private List<ProductType> _productTypeList;

        public ProductTypeLoader(DataTable productTypesTable)
        {
            _productTypeList = new List<ProductType>();
            _productTypesTable = productTypesTable;
            AddToProductTypeList(_defaultProductTypeList);
        }

        public ProductTypeLoader(DataTable productTypesTable, Dictionary<string, decimal> productTypeList)
        {
            _productTypeList = new List<ProductType>();
            _productTypesTable = productTypesTable;
            AddToProductTypeList(productTypeList);
        }

        private void AddToProductTypeList(Dictionary<string, decimal> productTypeList)
        {
            int i = 0;
            foreach (var productType in productTypeList)
            {
                _productTypeList.Add(new ProductType() { UPC = i, ProductName = productType.Key, Price = productType.Value });
                i++;
            }
        }

        public void LoadProductTypesToDB()
        {
            MapEntityToDatabase.MapProductTypeEntityToTable(_productTypeList, _productTypesTable);
        }

        private Dictionary<string, decimal> _defaultProductTypeList = new Dictionary<string, decimal>
        {
            { "Apples", 1.00m },
            { "Bacon", 5.50m },
            { "Bananas", 0.50m },
            { "Bread", 2.00m },
            { "Cake", 15.00m },
            { "Candy", 2.50m },
            { "Carrots", 1.00m },
            { "Celery", 1.00m },
            { "Chocolate", 1.00m },
            { "Coke", 0.75m },
            { "Cookies", 1.50m },
            { "Crackers", 1.50m },
            { "Donuts", 1.75m },
            { "Eggs", 3.50m },
            { "Gatorade", 1.00m },
            { "Ginger", 1.00m },
            { "Grapes", 4.00m },
            { "Honey", 6.00m },
            { "Ice_Cream", 5.00m },
            { "Ketchup", 3.00m },
            { "Onion", 0.0m },
            { "Orange", 1.80m },
            { "Other", 1.00m },
            { "Pizza", 7.00m },
            { "Spinach", 1.00m },
            { "Wine", 12.00m },
            { "Yogurt", 3.00m },
            { "Zucchini", 1.00m }
        };
    }
}
