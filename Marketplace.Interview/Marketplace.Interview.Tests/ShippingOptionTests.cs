using System.Collections.Generic;
using NUnit.Framework;
using Marketplace.Interview.Business.Basket;
using Marketplace.Interview.Business.Shipping;
using System.IO;
using System;
using System.Linq;
using Marketplace.Interview.Business;


namespace Marketplace.Interview.Tests
{
    [TestFixture]
    public class ShippingOptionTests
    {
        [Test]
        public void FlatRateShippingOptionTest()
        {
            var flatRateShippingOption = new FlatRateShipping {FlatRate = 1.5m};
            var shippingAmount = flatRateShippingOption.GetAmount(new LineItem(), new Basket());

            Assert.That(shippingAmount, Is.EqualTo(1.5m), "Flat rate shipping not correct.");
        }

        [Test]
        public void PerRegionShippingOptionTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
                                              {
                                                  PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
                                              };

            var shippingAmount = perRegionShippingOption.GetAmount(new LineItem() {DeliveryRegion = RegionShippingCost.Regions.Europe}, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(1.5m));

            shippingAmount = perRegionShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.UK}, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(.75m));
        }

        [Test]
        public void BasketShippingTotalTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
            };

            var flatRateShippingOption = new FlatRateShipping {FlatRate = 1.1m};

            var basket = new Basket()
                             {
                                 LineItems = new List<LineItem>
                                                 {
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.UK,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.Europe,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem() {Shipping = flatRateShippingOption},
                                                 }
                             };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.That(basketShipping, Is.EqualTo(3.35m));
        }

        [Test]
        public void NewCountryShippingOptionTest()
        {
            var perCountryShippingOption = new PerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 10m
                                                                               }
                                                                       },
            };


            var shippingAmount = perCountryShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.Europe }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(10.5m),"Shipping charge for Europe is wrong!");
        }

        [Test]
        public void TotalShippingTest()
        {
            var perShippingOption = new PerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .5m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 10m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.RestOfTheWorld,
                                                                                   Amount = 2m
                                                                               }
                                                                       },
            };

            var flatRateShippingOption = new FlatRateShipping { FlatRate = 1.5m };

            var basket = new Basket()
            {
                LineItems = new List<LineItem>
                                                 {
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.UK,
                                                             Shipping = perShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.Europe,
                                                             Shipping = perShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.Europe,
                                                             Shipping = perShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.RestOfTheWorld,
                                                             Shipping = perShippingOption
                                                         },
                                                     new LineItem() {Shipping = flatRateShippingOption},
                                                 }
            };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.That(basketShipping, Is.EqualTo(15.5m),"Total Amount doest not match");
        }


        [Test]
        public void CountryShippingTest()
        {
            IGetBasketQuery basketLoader = new GetBasketQuery();
            IAddToBasketCommand addToBasket = new AddToBasketCommand();
            IGetShippingOptionsQuery getShippingOptions = new GetShippingOptions();

            Dictionary<string, ShippingBase> shippingOptions =
                getShippingOptions.Invoke(new GetShippingOptionsRequest()).ShippingOptions;


            var lineItemList = new List<LineItem>
            {
                new LineItem
                {
                    Amount = 1.5m,
                    ProductId = "1",
                    Shipping = shippingOptions["CustomShipping"],
                    SupplierId = 1,
                    DeliveryRegion = RegionShippingCost.Regions.Europe,
                    ShippingAmount = 1m
                },
                new LineItem
                {
                    Amount = 1.5m,
                    ProductId = "1",
                    Shipping = shippingOptions["CustomShipping"],
                    SupplierId = 1,
                    DeliveryRegion = RegionShippingCost.Regions.Europe,
                    ShippingAmount = 0.5m
                },
                new LineItem
                {
                    Amount = 1.5m,
                    ProductId = "1",
                    Shipping = shippingOptions["CustomShipping"],
                    SupplierId = 1,
                    DeliveryRegion = RegionShippingCost.Regions.UK,
                    ShippingAmount = 0.5m
                },
                new LineItem
                {
                    Amount = 1.5m,
                    ProductId = "1",
                    Shipping = shippingOptions["CustomShipping"],
                    SupplierId = 1,
                    DeliveryRegion = RegionShippingCost.Regions.UK,
                    ShippingAmount = 0.0m
                },
                new LineItem
                {
                    Amount = 1.5m,
                    ProductId = "1",
                    Shipping = shippingOptions["CustomShipping"],
                    SupplierId = 1,
                    DeliveryRegion = RegionShippingCost.Regions.RestOfTheWorld,
                    ShippingAmount = 2m
                },
                new LineItem
                {
                    Amount = 1.5m,
                    ProductId = "1",
                    Shipping = shippingOptions["CustomShipping"],
                    SupplierId = 1,
                    DeliveryRegion = RegionShippingCost.Regions.RestOfTheWorld,
                    ShippingAmount = 1.5m
                }
            };

            foreach (LineItem item in lineItemList)
            {
                addToBasket.Invoke(new AddToBasketRequest { LineItem = item });
            }

            Basket basket = basketLoader.Invoke(new BasketRequest());

            foreach (LineItem item in basket.LineItems)
            {
                LineItem sampleListItem = lineItemList.FirstOrDefault(a => a.Id == item.Id);

                Assert.AreEqual(sampleListItem.SupplierId, item.SupplierId, "SupplierId are not equal");
                Assert.AreEqual(sampleListItem.DeliveryRegion, item.DeliveryRegion, "Region are not equal");
                Assert.AreEqual(sampleListItem.ShippingAmount, item.ShippingAmount, "Amount are not equal");
            }
        }
    }
}