using GroceryCo.Application.Processors;
using GroceryCo.Domain;
using GroceryCo.Infrastructure.Database;
using static GroceryCo.Infrastructure.Database.GroceryCoDatabase;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GroceryCo.Tests
{
    [TestFixture]
    public class MapEntityToDatabaseTests
    {
        [Test]
        public void DiscountTypeEntityToDBTest()
        {
            DiscountTypeDataTable discountTypeTable = new DiscountTypeDataTable();
            IGroceryCoTable<DiscountTypeDataTable> discountTable = new GroceryCoDatasetTable<DiscountTypeDataTable>(discountTypeTable);

            MapEntityToDatabase.MapDiscountTypesEntityToTable(discountTypeTable);

            Assert.AreEqual(2, discountTypeTable.Rows.Count);
        }

        [Test]
        public void ProductTypeEntityToDBTest()
        {
            Dictionary<string, decimal> items = new Dictionary<string, decimal>()
            {
                { "Apples", 1.00m },
                { "Oranges", 0.75m },
                { "Grapes", 3.00m }
            };

            ProductTypeDataTable productTypeTable = new ProductTypeDataTable();
            ProductTypeProcessor processor = new ProductTypeProcessor();
            processor.ProductTypes = items;
            processor.MapToEntities();
            processor.LoadEntityToDatabase(productTypeTable);

            Assert.AreEqual(3, productTypeTable.Rows.Count);
        }
    }
}
