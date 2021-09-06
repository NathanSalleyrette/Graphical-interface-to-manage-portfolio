using System;
using System.Collections.Generic;
using System.Linq;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.Utilities;

namespace SystematicStrategies.main
{
    class Program
    {

        public static void Main()
        {
            Share action = new Share("AC FP", "AC FP");
            SemiHistoricDataFeedProvider sdf = new SemiHistoricDataFeedProvider();
            double nbDaysPerYear = sdf.NumberOfDaysPerYear;
            string[] ids = new string[] { "AC FP" };
            List<DataFeed> dfList = sdf.GetDataFeed(ids , new DateTime(2010, 02, 05), new DateTime(2012, 12, 30));

            var firstMarket = dfList[0];
            dfList.RemoveAt(0);
            List<double> volatilities = new List<double>();
            double mean = 0;
            foreach(var df in dfList)
            {
                volatilities.Add( Math.Abs( Math.Log((double)(df.PriceList["AC FP"]) / (double)(firstMarket.PriceList["AC FP"])) )/ Math.Sqrt(1/ nbDaysPerYear ));
                
                firstMarket = df;
                mean += volatilities.Last();
            }

            Console.WriteLine(mean / dfList.Count);
            Console.ReadLine();
        }
    }
}
