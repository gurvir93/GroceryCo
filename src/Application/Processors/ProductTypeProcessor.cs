using GroceryCo.Domain;
using GroceryCo.Domain.Entities;
using GroceryCo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GroceryCo.Application.Processors
{
    public class ProductTypeProcessor : FileProcessor
    {
        private const string _defaultProductTypeFileLocation = "Files\\DefaultProductTypes.txt";
        private Dictionary<string, decimal> ProductTypes;
        public List<ProductTypeEntity> ProductTypeList { get; set; }

        public ProductTypeProcessor()
        {
            ProductTypes = new Dictionary<string, decimal>();
            FilePath = _defaultProductTypeFileLocation;

            ProductTypeList = new List<ProductTypeEntity>();
        }

        public bool HasProduct(int upc)
        {
            return ProductTypeList.Any(p => p.UPC == upc);
        }

        public bool HasProduct(string productName)
        {
            return ProductTypeList.Any(p => p.ProductName == productName);
        }

        public void AddProductType(string productName, decimal price)
        {
            int upc = ProductTypeList.Select(p => p.UPC).Max() + 1;
            ProductTypeList.Add(new ProductTypeEntity() { UPC = upc, ProductName = productName, Price = price });
        }

        public override bool ParseLines()
        {
            string[] lines = ReadFile();

            if (lines != null)
            {
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var values = line.Split(',').Select(v => v.Trim()).ToList();
                        if (values.Count == 2)
                        {
                            decimal price;

                            if (GroceryCoValidator.ValidateDecimal(values[1], out price))
                            {
                                ProductTypes.Add(values[0], price);
                            }
                            else
                            {
                                Console.WriteLine($"Invalid price entered for [{values[0]}].");
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Price not entered for [{values[0]}].");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void LoadProductListFromTable(DataTable table)
        {
            ProductTypeList = MapTableToEntity.MapToProductTypeEntity(table);
        }

        public void MapToEntities()
        {
            int i = 0;
            foreach (var productType in ProductTypes)
            {
                ProductTypeList.Add(new ProductTypeEntity() { UPC = i, ProductName = productType.Key, Price = productType.Value });
                i++;
            }
        }

        public void LoadEntityToDatabase(DataTable productTypeTable)
        {
            MapEntityToDatabase.MapProductTypeEntityToTable(ProductTypeList, productTypeTable);
        }
    }
}
