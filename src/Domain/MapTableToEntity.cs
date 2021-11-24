using GroceryCo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace GroceryCo.Domain
{
    public class MapTableToEntity
    {
        public static List<ProductEntity> MapToProductEntity(DataTable dataTable)
        {
            List<ProductEntity> productEntities = new List<ProductEntity>();

            foreach (DataRow row in dataTable.Rows)
            {
                productEntities.Add(new ProductEntity()
                {
                    UPC = (int) row[nameof(ProductEntity.UPC)],
                    Quantity = (int) row[nameof(ProductEntity.Quantity)]
                });
            }

            return productEntities;
        }

        public static List<ProductTypeEntity> MapToProductTypeEntity(DataTable dataTable)
        {
            List<ProductTypeEntity> productTypeEntities = new List<ProductTypeEntity>();

            foreach (DataRow row in dataTable.Rows)
            {
                productTypeEntities.Add(new ProductTypeEntity()
                {
                    UPC = (int) row[nameof(ProductTypeEntity.UPC)],
                    ProductName = row[nameof(ProductTypeEntity.ProductName)].ToString(),
                    Price = (decimal)row[nameof(ProductTypeEntity.Price)]
                });
            }

            return productTypeEntities;
        }

        public static List<DiscountEntity> MapToDiscountEntity(DataTable dataTable)
        {
            List<DiscountEntity> discountEntities = new List<DiscountEntity>();

            foreach (DataRow row in dataTable.Rows)
            {
                discountEntities.Add(new DiscountEntity()
                {
                     UPC = (int)row[nameof(DiscountEntity.UPC)],
                     DiscountType = (DiscountTypeIDs)Enum.Parse(typeof(DiscountTypeIDs), row[nameof(DiscountEntity.DiscountTypeID)].ToString()),
                     StartDate = (DateTime)row[nameof(DiscountEntity.StartDate)],
                     EndDate = (DateTime)row[nameof(DiscountEntity.EndDate)],
                     DiscountPercent = int.Parse(row[nameof(DiscountEntity.DiscountPercent)].ToString()),
                     ItemsRequired = int.Parse(row[nameof(DiscountEntity.ItemsRequired)].ToString())
                });
            }

            return discountEntities;
        }
    }
}
