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
            Share action = new Share("AC FP", "AC FP");
            SemiHistoricDataFeedProvider sdf = new SemiHistoricDataFeedProvider();
            double nbDaysPerYear = sdf.NumberOfDaysPerYear;
            string[] ids = new string[] { "AC FP" };
            List<DataFeed> dfList = sdf.GetDataFeed(ids , new DateTime(2010, 01, 05), new DateTime(2010, 10, 30));
            
            foreach(DataFeed df in dfList)
            {
                Console.Write(df.Date);
                Console.Write("\t");
                Console.Write(string.Join(Environment.NewLine, df.PriceList));
                Console.Write("\n");
            }

            var strike = 12;
            VanillaCall opt = new VanillaCall("VCall", action, new DateTime(2010, 10, 30), strike);
            var Pricer = new Pricer();

            var currentDate = dfList[0].Date;

            var cours = decimal.ToDouble(dfList[0].PriceList["AC FP"]);  // égale à 10
            PricingResults result = Pricer.Price(opt, currentDate, (int) nbDaysPerYear, cours, 0.25);
            
            double V = result.Price;
            var S = cours;
            var delta = result.Deltas[0];
            var investTauxSS = V - delta * S;
            Console.Write(S);
            Console.Write("\n");
            Console.Write(delta);
            Console.Write("\n");
            dfList.RemoveAt(0);

            foreach (DataFeed df in dfList)
            {
                double dayCount = DayCount.CountBusinessDays(currentDate, df.Date);
                double riskRate = RiskFreeRateProvider.GetRiskFreeRateAccruedValue(dayCount/nbDaysPerYear);
                Console.Write(riskRate);
                Console.Write("\t");
                Console.Write(currentDate);
                Console.Write("\t");
                Console.Write("Cours de l'action : ");
                Console.Write(cours);
                Console.Write("\t");
                Console.Write("V : ");
                Console.Write(V);
                Console.Write("\t");
                Console.Write("Investissement taux SS : ");
                Console.Write(investTauxSS);
                Console.Write("\t");
                Console.Write("delta : ");
                Console.Write(delta);
                Console.Write("\n");


                cours = decimal.ToDouble(df.PriceList["AC FP"]);
                result = Pricer.Price(opt, df.Date, (int) nbDaysPerYear, cours, 0.25);
                S = cours;
                V = investTauxSS * riskRate + delta * S;
                currentDate = df.Date;
                delta = result.Deltas[0];
                investTauxSS = V - delta * S;

                
            }

            var resultFinal = V - Math.Max(cours - strike, 0);

            Console.WriteLine(resultFinal);

            var trackingError = resultFinal / result.Price;

            Console.Write("Tracking Error : ");
            Console.Write(trackingError);
            Console.ReadLine();


        }
    }
}
