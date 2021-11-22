using GroceryCo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace GroceryCo.Domain
{
    public class MapEntityToDatabase
    {
        public static void MapProductEntityToTable<T>(List<Product> productList, DataTable dataTable)
        {
            foreach (var entity in productList)
            {
                DataRow row = dataTable.NewRow();
                row[nameof(entity.UPC)] = entity.UPC;

                dataTable.Rows.Add(row);
            }
        }

        public static void MapProductTypeEntityToTable(List<ProductType> productTypes, DataTable dataTable)
        {
            foreach (var productType in productTypes)
            {
                DataRow row = dataTable.NewRow();
                row[nameof(productType.UPC)] = productType.UPC;
                row[nameof(productType.ProductName)] = productType.ProductName;

                dataTable.Rows.Add(row);
            }
        }

        public static void MapDiscountEntityToTable(List<Discount> discountList, DataTable dataTable)
        {
            foreach (var entity in discountList)
            {
                DataRow row = dataTable.NewRow();
                row[nameof(entity.UPC)] = entity.UPC;
                row[nameof(entity.DiscountTypeID)] = entity.DiscountTypeID;
                row[nameof(entity.StartDate)] = entity.StartDate;
                row[nameof(entity.EndDate)] = entity.EndDate;
                row[nameof(entity.DiscountPercentage)] = entity.DiscountPercentage;
                row[nameof(entity.ItemsRequired)] = entity.ItemsRequired;

                dataTable.Rows.Add(row);
            }
        }

        public static void MapDiscountTypesEntityToTable(DataTable dataTable)
        {
            foreach (DiscountTypeIDs value in Enum.GetValues(typeof(DiscountTypeIDs)))
            {
                DataRow row = dataTable.NewRow();
                row["DiscountTypeID"] = value.ToString();

                dataTable.Rows.Add(row);
            }
        }
    }
}
