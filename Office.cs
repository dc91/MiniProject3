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
        public decimal ConversionRate { get; set; }

        public string Currency { get; set; }
        public List<Asset> Assets { get; set; } = [];

        public Office(string name, decimal conversionRate, string currency) 
        {
            Name = name;
            ConversionRate = conversionRate;
            Currency = currency;
        }

        public void AddAsset(Asset asset)
        {
            Assets.Add(asset);
        }

        public decimal ConvertPriceFromUSD(decimal price)
        {
            return price * ConversionRate;
        }

        public void UpdateCurrencyConversionRate(decimal newRate)
        {
            ConversionRate = newRate;
        }
    }
}
