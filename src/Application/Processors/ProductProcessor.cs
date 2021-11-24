using GroceryCo.Domain.Entities;
using GroceryCo.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GroceryCo.Application.Processors
{
    public class ProductProcessor : FileProcessor
    {
        public Dictionary<int, int> Products;
        private List<ProductTypeEntity> _productTypes;
        private List<ProductEntity> _productEntities;

        public ProductProcessor(List<ProductTypeEntity> productTypes)
        {
            _productEntities = new List<ProductEntity>();
            Products = new Dictionary<int, int>();
            _productTypes = productTypes;
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
                        ProductTypeEntity current = _productTypes.FirstOrDefault(t => t.ProductName.Equals(line, StringComparison.CurrentCultureIgnoreCase));
                        if (current != null)
                        {
                            if(Products.ContainsKey(current.UPC))
                            {
                                Products[current.UPC]++;
                            }
                            else
                            {
                                Products.Add(current.UPC, 1);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Item [{line}] is not a valid product type.");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void RemoveDiscountsByUPC(int upc)
        {
            _productEntities.RemoveAll(p => p.UPC == upc);
        }

        public void MapToEntities()
        {
            int i = 0;
            foreach (var product in Products)
            {
                _productEntities.Add(new ProductEntity() { UPC = product.Key, Quantity = product.Value });
                i++;
            }
        }

        public void LoadEntityToDatabase(DataTable productTable)
        {
            MapEntityToDatabase.MapProductEntityToTable(_productEntities, productTable);
        }
    }
}
