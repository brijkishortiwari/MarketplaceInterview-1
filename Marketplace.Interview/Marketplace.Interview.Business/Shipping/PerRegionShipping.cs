﻿using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class PerRegionShipping : ShippingBase
    {
        public IEnumerable<RegionShippingCost> PerRegionCosts { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Shipping to {0}", lineItem.DeliveryRegion);
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            
            //List<RegionShippingCost> listPerRegionCosts = PerRegionCosts.ToList();
            //bool containsCountryRegion = listPerRegionCosts.Exists(x=>x.DestinationRegion==lineItem.DeliveryRegion);
            ////bool checkListContains=(from c in PerRegionCosts where c.DestinationRegion.Contains(lineItem.DeliveryRegion));
            //if (containsCountryRegion)
            //    return
            //    (from c in PerRegionCosts
            //     where c.DestinationRegion == lineItem.DeliveryRegion
            //     select c.Amount).Single();

            //else
            //{
            //    lineItem.DeliveryRegion = RegionShippingCost.Regions.RestOfTheWorld;
            //    return
            //    (from c in PerRegionCosts
            //     where c.DestinationRegion == lineItem.DeliveryRegion
            //     select c.Amount).Single();
            //}

            return
            (from c in PerRegionCosts
             where c.DestinationRegion == lineItem.DeliveryRegion
             select c.Amount).Single();
            
        }
    }
}