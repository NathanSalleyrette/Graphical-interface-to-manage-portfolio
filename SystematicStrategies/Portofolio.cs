using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.Strategies;

namespace SystematicStrategies
{
    class Portofolio
    {
        double value;
        Dictionary<string, double> assetWeights;
        double investmentFreeRiskRate = 0;

        public Portofolio(Option option, Strategy strategy, DataFeed dataFeed, List<DataFeed> dataFeedList, int nbOfDaysPerYear)
        {
            Pricer pricer = new Pricer();
            value = (option is VanillaCall) ? pricer.Price((VanillaCall)option, dataFeed.Date, nbOfDaysPerYear, decimal.ToDouble(dataFeed.PriceList[option.UnderlyingShareIds[0]]), 0.25).Price : 0.0;
            assetWeights = strategy.UpdateCompo(option, dataFeed, dataFeedList, nbOfDaysPerYear);
            investmentFreeRiskRate = value;
            foreach (var asset in assetWeights.Keys)
            {
                investmentFreeRiskRate -= assetWeights[asset] * (double)dataFeed.PriceList[asset];
            }

        }
        public void update(Option option, Strategy strategy, DataFeed dataFeed, List<DataFeed> dataFeedList, int nbOfDaysPerYear, double riskRate)
        {
            value = investmentFreeRiskRate * riskRate;
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
