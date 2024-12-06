using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject3
{
    internal abstract class Asset
    {
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public string Office { get; set; }
        public string EndOfLifeStatus 
        {
            get
            {
                // Calculate the end of life status based on the purchase date
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                DateOnly endOfLifeDate = PurchaseDate.AddMonths(36);

                if (currentDate > endOfLifeDate)
                    return "Past End of Life";
                else if (currentDate > endOfLifeDate.AddMonths(-3))
                    return "3 Months Left";
                else if (currentDate > endOfLifeDate.AddMonths(-6))
                    return "6 Months Left";
                else
                    return "Normal";
            }

        }
        public abstract string Type { get; } // protected so children can reach it

        public Asset(string manufacturer, string modelname, decimal price, string currency, DateOnly purchaseDate, string office)
        {
            Manufacturer = manufacturer;
            ModelName = modelname;
            Price = price;
            Currency = currency;
            PurchaseDate = purchaseDate;
            Office = office;
        }
    }


    internal class Laptop(string manufacturer, string modelName, decimal price, string currency, DateOnly purchaseDate, string office) 
        : Asset(manufacturer, modelName, price, currency, purchaseDate, office)
    {
        public override string Type => "Laptop";
    }

    internal class MobilePhone(string manufacturer, string modelName, decimal price, string currency, DateOnly purchaseDate, string office) 
        : Asset(manufacturer, modelName, price, currency, purchaseDate, office)
    {
        public override string Type => "MobilePhone";
    }

}
