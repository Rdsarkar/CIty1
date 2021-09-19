using System;
using System.Collections.Generic;

#nullable disable

namespace CIty1.Models
{
    public partial class City
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Countrycode { get; set; }
        public decimal? Population { get; set; }
    }
}
