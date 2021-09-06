using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.Strategies;

namespace SystematicStrategies.Portfolio
{
    internal abstract class AbstractPortfolio
    {
        public double value;
        protected Dictionary<string, double> assetWeights;
        protected double investmentFreeRiskRate = 0;

        
            //Pricer pricer = new Pricer();
            //if (option is VanillaCall)
            //{
            //    value = pricer.Price((VanillaCall)option, dataFeed.Date, nbOfDaysPerYear, decimal.ToDouble(dataFeed.PriceList[option.UnderlyingShareIds[0]]), 0.25).Price;
            //} else
            //{
            //    var n = option.UnderlyingShareIds.Length;
            //    double[] spots = new double[n];
            //    double[] volatilities = new double[n];
            //    double[,] corMatrix = new double[n, n];
            //    for (var j = 0; j < n * n; j++) corMatrix[j % n, j / n] = 0.15;
            //    var i = 0;
            //    foreach (string UnderlyingShareId in option.UnderlyingShareIds)
            //    {
            //        volatilities[i] = 0.25;
            //        corMatrix[i, i] = 0.25;
            //        spots[i] = decimal.ToDouble(dataFeed.PriceList[UnderlyingShareId]);
            //        i += 1;
            //    };
            //    value = pricer.Price((BasketOption)option, dataFeed.Date, nbOfDaysPerYear, spots, volatilities, corMatrix).Price;
            //}

            //assetWeights = strategy.UpdateCompo(option, dataFeed, dataFeedList, nbOfDaysPerYear);
            //investmentFreeRiskRate = value;
            //foreach (var asset in assetWeights.Keys)
            //{
            //    investmentFreeRiskRate -= assetWeights[asset] * (double)dataFeed.PriceList[asset];
            //}
            //Console.WriteLine(value);

        

        public abstract void Initialize(IOption option, IStrategy strategy, DataFeed dataFeed, List<DataFeed> dataFeedList, int nbOfDaysPerYear);
        public void Update(IOption option, IStrategy strategy, DataFeed dataFeed, List<DataFeed> dataFeedList, int nbOfDaysPerYear, double riskRate)
        {
            
            value = investmentFreeRiskRate * riskRate;
            Console.Write("Portofolio value : ");
            Console.WriteLine(value);
            Console.Write("investment freeRisk");
            Console.WriteLine(investmentFreeRiskRate);
            foreach(var id in assetWeights.Keys)
            {
                Console.Write(id, " : ");
                Console.WriteLine(dataFeed.PriceList[id]);

            }
            Console.WriteLine(string.Join(Environment.NewLine, assetWeights));
            foreach (var asset in assetWeights.Keys)
            {
                value += assetWeights[asset] * (double)dataFeed.PriceList[asset];
            }
            assetWeights = strategy.UpdateCompo(option, dataFeed, dataFeedList, nbOfDaysPerYear);
            investmentFreeRiskRate = value;
            foreach (var asset in assetWeights.Keys)
            {
                investmentFreeRiskRate -= assetWeights[asset] * (double)dataFeed.PriceList[asset];
            }
        }
    }
}
