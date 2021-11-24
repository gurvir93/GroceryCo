using GroceryCo.Application.Processors;
using GroceryCo.Domain;
using GroceryCo.Domain.Entities;
using static GroceryCo.Infrastructure.Database.GroceryCoDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GroceryCo.Tests
{
    [TestFixture]
    public class MapEntityToDatabaseAndProcessorTests
    {
        [Test]
        public void DiscountTypeEntityToDBTest()
        {
            DiscountTypeDataTable discountTypeTable = new DiscountTypeDataTable();
            MapEntityToDatabase.MapDiscountTypesEntityToTable(discountTypeTable);

            Assert.AreEqual(2, discountTypeTable.Rows.Count);
        }

        [Test]
        public void ProductTypeEntityToDBTest()
        {
            List<ProductTypeEntity> items = new List<ProductTypeEntity>()
            {
                new ProductTypeEntity() { UPC = 0, ProductName = "Apples", Price = 1.00m },
                new ProductTypeEntity() { UPC = 1, ProductName = "Oranges", Price = 0.75m },
                new ProductTypeEntity() { UPC = 2, ProductName = "Grapes", Price = 3.00m }
            };

            ProductTypeDataTable productTypeTable = new ProductTypeDataTable();
            ProductTypeProcessor processor = new ProductTypeProcessor();

            processor.ProductTypeList = items;
            processor.MapToEntities();
            processor.LoadEntityToDatabase(productTypeTable);

            Assert.AreEqual(3, productTypeTable.Rows.Count);
        }

        [Test]
        public void ProductEntityToDBTest()
        {
            List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>()
            {
                new ProductTypeEntity() { UPC = 0, ProductName = "Apples", Price = 1.00m },
                new ProductTypeEntity() { UPC = 1, ProductName = "Oranges", Price = 0.75m },
                new ProductTypeEntity() { UPC = 2, ProductName = "Grapes", Price = 3.00m },
                new ProductTypeEntity() { UPC = 3, ProductName = "Peach", Price = 1.25m }
            };

            Dictionary<int, int> items = new Dictionary<int, int>()
            {
                { 0, 1 }, { 1, 5 }, { 2, 2 }, { 3, 1 }
            };

            ProductDataTable productTable = new ProductDataTable();
            ProductProcessor processor = new ProductProcessor(productTypes);

            processor.Products = items;
            processor.MapToEntities();
            processor.LoadEntityToDatabase(productTable);

            Assert.AreEqual(4, productTable.Rows.Count);
        }

        [Test]
        public void DiscountEntityToDBTest()
        {
            DiscountDataTable discountTable = new DiscountDataTable();
            DiscountProcessor processor = new DiscountProcessor();

            processor.AddDiscount(new DiscountEntity() { UPC = 0, DiscountType = DiscountTypeIDs.PercentOff, DiscountPercent = 50, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) });
            processor.AddDiscount(new DiscountEntity() { UPC = 2, DiscountType = DiscountTypeIDs.BuyXGetY, ItemsRequired = 2, DiscountPercent = 50, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) });
            processor.MapEntitiesToTable(discountTable);

            Assert.AreEqual(2, discountTable.Rows.Count);
        }
    }
}
