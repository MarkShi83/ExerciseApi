namespace ExerciseApi.Model
{
    using System.Collections.Generic;

    public class Trolley
    {
        public Product[] Products { get; set; }

        public Special[] Specials { get; set; }

        public ProductQuantity[] Quantities { get; set; }
    }
}
