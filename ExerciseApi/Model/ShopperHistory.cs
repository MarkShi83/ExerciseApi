﻿namespace ExerciseApi.Model
{
    using System.Collections.Generic;

    public class ShopperHistory
    {
        public int CustomerId { get; set; }

        public List<Product> Products { get; set; }
    }
}
