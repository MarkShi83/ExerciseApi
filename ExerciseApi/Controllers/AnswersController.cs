using Newtonsoft.Json;

namespace ExerciseApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;


    using Microsoft.Extensions.Logging;

    using ExerciseApi.Helper;
    using ExerciseApi.Model;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
	[ApiController]
	public class AnswersController : ControllerBase
	{
        private readonly ILogger<AnswersController> _logger;

        public AnswersController(ILogger<AnswersController> logger)
        {
            _logger = logger;
        }
        [HttpGet("user")]
        public ActionResult<User> Get()
        {
            return new User { Name = "Mark Shi", Token = "87c30bff-0669-4dcb-a5da-ae240fe9cdd7" };
        }


        [HttpGet("sort")]
		public ActionResult<List<Product>> Get([FromQuery]string sortOption)
        {
            var products = HttpRequestHelper.GetProducts();
            switch (sortOption.ToLowerInvariant())
            {
                case "low":
                    return products.Result.OrderBy(p => p.Price).ToList();
                case "high":
                    return products.Result.OrderByDescending(p => p.Price).ToList();
                case "ascending":
                    return products.Result.OrderBy(p => p.Name).ToList();
                case "descending":
                    return products.Result.OrderByDescending(p => p.Name).ToList();
                case "recommended":
                    var shopperHistories = HttpRequestHelper.GetShopperHistories();

                    var productPopularities = products.Result.Select(
                        x => new ProductPopularity
                                 {
                                     Popularity = shopperHistories.Result.Count(s => s.Products.Exists(p => p.Name == x.Name)),
                                     Product = x
                                 });
                    var sortedProducts = productPopularities.OrderByDescending(x => x.Popularity).Select(x => x.Product);

                    return sortedProducts.ToList();
                default:
                    return null;
            }
        }


        [HttpPost("trolleyTotal")]
        public decimal Post([FromBody] Trolley trolley)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(trolley));
            //return HttpRequestHelper.PostTrolleyCalculator(trolley).Result;
            return HttpRequestHelper.CalculateTotal(trolley);
        }
    }
}
