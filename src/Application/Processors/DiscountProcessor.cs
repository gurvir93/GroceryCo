using GroceryCo.Domain;
using GroceryCo.Domain.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GroceryCo.Application.Processors
{
    public class DiscountProcessor
    {
        public List<DiscountEntity> DiscountEntities;

        public DiscountProcessor()
        {
            DiscountEntities = new List<DiscountEntity>();
        }
        public void AddDiscount(DiscountEntity entity)
        {
            DiscountEntity prevEntity = DiscountEntities.FirstOrDefault(d => d.UPC == entity.UPC
                                                                            && d.DiscountTypeID == entity.DiscountTypeID
                                                                            && d.StartDate == entity.StartDate);
            if (prevEntity != null)
            {
                prevEntity.EndDate = entity.EndDate;
                prevEntity.DiscountPercent = entity.DiscountPercent;
                prevEntity.ItemsRequired = entity.ItemsRequired;
            }
            else
            {
                DiscountEntities.Add(entity);
            }
        }

        public void RemoveDiscountsByUPC(int upc)
        {
            DiscountEntities.RemoveAll(d => d.UPC == upc);
        }

        public void MapEntitiesToTable(DataTable discountTable)
        {
            MapEntityToDatabase.MapDiscountEntityToTable(DiscountEntities, discountTable);
        }


    }
}
