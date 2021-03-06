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



        public abstract void Initialize(IOption option, IStrategy strategy, DataFeed dataFeed, List<DataFeed> dataFeedList, int nbOfDaysPerYear, double[] volatilities, double[,] corMatrix);
        public void Update(IOption option, IStrategy strategy, DataFeed dataFeed, List<DataFeed> dataFeedList, int nbOfDaysPerYear, double riskRate, double[] volatilities, double[,] corMatrix, bool rebalancing)
        {
            
            value = investmentFreeRiskRate * riskRate;

            foreach (var asset in assetWeights.Keys)
            {
                value += assetWeights[asset] * (double)dataFeed.PriceList[asset];
            }
            var weights = strategy.UpdateCompo(option, dataFeed, dataFeedList, nbOfDaysPerYear, volatilities, corMatrix);

            if (rebalancing)
            {
                assetWeights = weights;
                investmentFreeRiskRate = value;
                foreach (var asset in assetWeights.Keys)
                {
                    investmentFreeRiskRate -= assetWeights[asset] * (double)dataFeed.PriceList[asset];
                }
            }
        }
    }
}
