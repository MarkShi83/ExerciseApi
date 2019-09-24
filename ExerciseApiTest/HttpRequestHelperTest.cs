using ExerciseApi.Model;

namespace ExerciseApiTest
{
    using System.Linq;

    using ExerciseApi.Helper;

    using FluentAssertions;

    using Xunit;

    public class HttpRequestHelperTests
    {
        [Fact]
        public async void WhenGetProductsCalledItReturnsNotEmptyProductList()
        {
            var result = await HttpRequestHelper.GetProducts();
            result.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async void WhenGetShopperHistoryCalledItReturnsNotEmptyShopperList()
        {
            var result = await HttpRequestHelper.GetShopperHistories();
            result.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async void WhenPostTrolleyCalculatorCalledItReturnsCorrectTotalAmount()
        {
            var trolley = new Trolley
                              {
                                  Products = new[] { new Product { Name = "product1", Price = 1 } },
                                  Specials = new[]
                                                 {
                                                     new Special
                                                         {
                                                             Quantities =
                                                                 new[]
                                                                     {
                                                                         new ProductQuantity
                                                                             {
                                                                                 Name = "product1", Quantity = 2
                                                                             }
                                                                     },
                                                             Total = 1.5m
                                                         }
                                                 },
                                  Quantities = new[]
                                                   {
                                                       new ProductQuantity { Name = "product1", Quantity = 4 }
                                                   }
                              };

            var result = await HttpRequestHelper.PostTrolleyCalculator(trolley);
            result.Should().Be(3);
        }
    }
}