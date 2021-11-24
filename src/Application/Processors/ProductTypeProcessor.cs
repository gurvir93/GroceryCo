using GroceryCo.Domain;
using GroceryCo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GroceryCo.Application.Processors
{
    public class ProductTypeProcessor : FileProcessor
    {
        private const string _defaultProductTypeFileLocation = "Files\\DefaultProductTypes.txt";
        public Dictionary<string, decimal> ProductTypes;
        private List<ProductTypeEntity> _productTypeList;

        public ProductTypeProcessor()
        {
            ProductTypes = new Dictionary<string, decimal>();
            FilePath = _defaultProductTypeFileLocation;

            _productTypeList = new List<ProductTypeEntity>();
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

        public void MapToEntities()
        {
            int i = 0;
            foreach (var productType in ProductTypes)
            {
                _productTypeList.Add(new ProductTypeEntity() { UPC = i, ProductName = productType.Key, Price = productType.Value });
                i++;
            }
        }

        public void LoadEntityToDatabase(DataTable productTypeTable)
        {
            MapEntityToDatabase.MapProductTypeEntityToTable(_productTypeList, productTypeTable);
        }
    }
}
