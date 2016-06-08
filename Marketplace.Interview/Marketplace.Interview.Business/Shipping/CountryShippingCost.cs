using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Interview.Business.Shipping
{
    public class CountryShippingCost
    {

        public string DestinationRegion { get; set; }
        public decimal Amount { get; set; }

        public static class Regions
        {
            public const string UK = "UK";
            public const string Europe = "Europe";
            public const string RestOfTheWorld = "RestOfTheWorld";
        }
    }
}
