using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.Computations;
using PricingLibrary.Utilities;

namespace SystematicStrategies.main
{
    class Program
    {

        public static void Main()
        {
            Share action = new Share("AIREBUS", "0");
            SimulatedDataFeedProvider sdf = new SimulatedDataFeedProvider();
            string[] ids = new string[] { "0" };
            List<DataFeed> dfList = sdf.GetDataFeed(ids , new DateTime(2010, 01, 05), new DateTime(2010, 10, 30));
            
            foreach(DataFeed df in dfList)
            {
                Console.Write(df.Date);
                Console.Write("\t");
                Console.Write(string.Join(Environment.NewLine, df.PriceList));
                Console.Write("\n");
            }

            VanillaCall opt = new VanillaCall("VCall", action, new DateTime(2010, 10, 30), 10.5);
            var Pricer = new Pricer();

            var V0 = 100;
            var T0 = new DateTime(2010, 01, 05);
            var cours0 = decimal.ToDouble(dfList[0].PriceList["0"]);  // égale à 10
            PricingResults result0 = Pricer.Price(opt, T0, 360, cours0, 0.25);

            var S0 = result0.Price;
            var delta0 = result0.Deltas[0];

            Console.Write(S0);
            Console.Write("\n");
            Console.Write(delta0);
            Console.Write("\n");





        }
    }
}
