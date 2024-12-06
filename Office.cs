using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject3
{
    internal class Office
    {
        public string Name { get; set; }
        public List<KeyValuePair<string, decimal>> ConversionRates { get; set; }

        public string Currency { get; set; }
        public List<Asset> Assets { get; set; } = [];

        public Office(string name, List<KeyValuePair<string, decimal>> conversionRates, string currency) 
        {
            Name = name;
            ConversionRates = conversionRates;
            Currency = currency;
        }

        public void AddAsset(Asset asset)
        {
            Assets.Add(asset);
        }

        public decimal ConvertPrice(decimal price, string fromToCurrency)
        {
           return ConversionRates.FirstOrDefault(p => p.Key == fromToCurrency).Value * price;
        }

    }
}
