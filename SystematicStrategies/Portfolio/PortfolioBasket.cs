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
    internal class PortfolioBasket : AbstractPortfolio
    {

        public override void Initialize(IOption option, IStrategy strategy, DataFeed dataFeed, List<DataFeed> dataFeedList, int nbOfDaysPerYear, double[] volatilities, double[,] corMatrix)
        {
            Pricer pricer = new Pricer();
            var n = option.UnderlyingShareIds.Length;
            double[] spots = new double[n];

            var i = 0;
            foreach (string UnderlyingShareId in option.UnderlyingShareIds)
            {

                spots[i] = decimal.ToDouble(dataFeed.PriceList[UnderlyingShareId]);
                i += 1;
            };
            value = pricer.Price((BasketOption)option, dataFeed.Date, nbOfDaysPerYear, spots, volatilities, corMatrix).Price;


            assetWeights = strategy.UpdateCompo((BasketOption)option, dataFeed, dataFeedList, nbOfDaysPerYear, volatilities, corMatrix);
            investmentFreeRiskRate = value;
            foreach (var asset in assetWeights.Keys)
            {
                investmentFreeRiskRate -= assetWeights[asset] * (double)dataFeed.PriceList[asset];
            }

        }
    }
}
