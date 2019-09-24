using System.Linq;

namespace ExerciseApi.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using ExerciseApi.Model;

    using Newtonsoft.Json;

    public static class HttpRequestHelper
    {
        public static async Task<IEnumerable<Product>> GetProducts()
        {
            IEnumerable<Product> products = null;

            var client = new HttpClient
                             {
                                 BaseAddress = new Uri(
                                     "http://dev-wooliesx-recruitment.azurewebsites.net")
                             };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var response = await client.GetAsync("/api/resource/products?token=87c30bff-0669-4dcb-a5da-ae240fe9cdd7");
            if (response.IsSuccessStatusCode)
            {
                products = JsonConvert.DeserializeObject<IEnumerable<Product>>(await response.Content.ReadAsStringAsync());
            }

            return products;
        }

        public static async Task<IEnumerable<ShopperHistory>> GetShopperHistories()
        {
            IEnumerable<ShopperHistory> shopperHistories = null;

            var client = new HttpClient
                             {
                                 BaseAddress = new Uri(
                                     "http://dev-wooliesx-recruitment.azurewebsites.net")
                             };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync("/api/resource/shopperHistory?token=87c30bff-0669-4dcb-a5da-ae240fe9cdd7");
            if (response.IsSuccessStatusCode)
            {
                shopperHistories = JsonConvert.DeserializeObject<IEnumerable<ShopperHistory>>(await response.Content.ReadAsStringAsync());
            }

            return shopperHistories;
        }

        public static async Task<decimal> PostTrolleyCalculator(Trolley trolley)
        {
            var total = 0m;

            var client = new HttpClient
                             {
                                 BaseAddress = new Uri(
                                     "http://dev-wooliesx-recruitment.azurewebsites.net")
                             };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = JsonConvert.SerializeObject(trolley);
            
            Console.WriteLine(content);

            var stringContent = new StringContent(
                content,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/resource/trolleyCalculator?token=87c30bff-0669-4dcb-a5da-ae240fe9cdd7", stringContent);
            if (response.IsSuccessStatusCode)
            {
                total = JsonConvert.DeserializeObject<decimal>(await response.Content.ReadAsStringAsync());
            }

            return total;
        }

        public static decimal CalculateTotal(Trolley trolley)
        {
            var totals = new List<decimal>();
            if (trolley.Products.Length == 0 || trolley.Quantities.Length == 0)
            {
                return 0m;
            }

            if (trolley.Specials.Length == 0)
            {
              return trolley.Products.Sum(p => p.Price * trolley.Quantities.First(q => q.Name == p.Name).Quantity);
            }
            else
            {
                foreach (var special in trolley.Specials)
                {
                    var min = special.Quantities.Min(
                        s => s.Quantity == 0 ? int.MaxValue : Math.Floor((decimal)(trolley.Quantities.First(q => q.Name == s.Name).Quantity / s.Quantity)));


                    var total = trolley.Quantities.Sum(q => (q.Quantity - min * special.Quantities.First(s => s.Name == q.Name).Quantity) * trolley.Products.First(p => p.Name == q.Name).Price) + special.Total * min;

                    totals.Add(total);
                }
            }

            return totals.Min();
        }
    }
}
