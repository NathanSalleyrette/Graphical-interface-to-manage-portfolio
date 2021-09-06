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
    internal class PortfolioVanilla : AbstractPortfolio
    {
        public override void Initialize(IOption option, IStrategy strategy, DataFeed dataFeed, List<DataFeed> dataFeedList, int nbOfDaysPerYear)
        {
            Pricer pricer = new Pricer();
            value = pricer.Price((VanillaCall) option, dataFeed.Date, nbOfDaysPerYear, decimal.ToDouble(dataFeed.PriceList[option.UnderlyingShareIds[0]]), 0.25).Price;
            assetWeights = strategy.UpdateCompo((VanillaCall) option, dataFeed, dataFeedList, nbOfDaysPerYear);
            investmentFreeRiskRate = value;
            foreach (var asset in assetWeights.Keys)
            {
                investmentFreeRiskRate -= assetWeights[asset] * (double)dataFeed.PriceList[asset];
            }

        }
    }
}
