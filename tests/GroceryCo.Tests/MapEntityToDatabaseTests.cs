using GroceryCo.Domain;
using static GroceryCo.Infrastructure.Database.GroceryCoDatabase;
using NUnit.Framework;
using System;

namespace GroceryCo.Tests
{
    [TestFixture]
    public class MapEntityToDatabaseTests
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
            ProductTypeDataTable productTypeTable = new ProductTypeDataTable();
            ProductTypeLoader productTypeLoader = new ProductTypeLoader(productTypeTable);

            productTypeLoader.LoadProductTypesToDB();

            Assert.AreEqual(28, productTypeTable.Rows.Count);
        }
    }
}
