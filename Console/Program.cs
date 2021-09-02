using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;

namespace SystematicStrategies.main
{
    class Program
    {

        public static void Main()
        {
            VanillaCall n = new VanillaCall("oui", new Share("loic", "1"), new DateTime(), 23.0);
            Share action = new Share("AIREBUS", "0");
            ShareValue[] cours = new ShareValue[30];
            for (int i = 0; i < 30; i++)
            {
                cours[i] = new ShareValue("0", new DateTime(2000, 01, i + 1), i);
                Console.Write(cours[i].DateOfPrice);
            }
            VanillaCall opt = new VanillaCall("Call", action, new DateTime(2000, 01, 30), 25); 

        }
    }
}
