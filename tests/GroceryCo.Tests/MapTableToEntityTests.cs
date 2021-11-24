using GroceryCo.Application.Processors;
using GroceryCo.Domain;
using GroceryCo.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GroceryCo.Tests
{
    [TestFixture]
    public class MapTableToEntityTests
    {
        private GroceryCoDatabase _gcDatabase;

        [OneTimeSetUp]
        public void SetUp()
        {
            Database db = new Database();
            _gcDatabase = new GroceryCoDatabase(db);
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

            ProductTypeProcessor processor = new ProductTypeProcessor();

            processor.ProductTypeList = items;
            processor.MapToEntities();
            processor.LoadEntityToDatabase(_gcDatabase.ProductTypeTable);

            List<ProductTypeEntity> dbEntities = MapTableToEntity.MapToProductTypeEntity(_gcDatabase.ProductTypeTable);

            Assert.AreEqual(3, dbEntities.Count);
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

            ProductProcessor processor = new ProductProcessor(productTypes);

            processor.Products = items;
            processor.MapToEntities();
            processor.LoadEntityToDatabase(_gcDatabase.ProductTable);

            List<ProductEntity> dbEntities = MapTableToEntity.MapToProductEntity(_gcDatabase.ProductTable);

            Assert.AreEqual(4, dbEntities.Count);
        }

        [Test]
        public void DiscountEntityToDBTest()
        {
            DiscountProcessor processor = new DiscountProcessor();

            processor.AddDiscount(new DiscountEntity() { UPC = 0, DiscountType = DiscountTypeIDs.PercentOff, DiscountPercent = 50, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) });
            processor.AddDiscount(new DiscountEntity() { UPC = 2, DiscountType = DiscountTypeIDs.BuyXGetY, ItemsRequired = 2, DiscountPercent = 50, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) });
            processor.MapEntitiesToTable(_gcDatabase.DiscountDataTable);

            List<DiscountEntity> dbEntities = MapTableToEntity.MapToDiscountEntity(_gcDatabase.DiscountDataTable);

            Assert.AreEqual(2, dbEntities.Count);
        }
    }
}
