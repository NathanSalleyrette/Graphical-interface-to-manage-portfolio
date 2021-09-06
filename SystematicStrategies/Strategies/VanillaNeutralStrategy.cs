using PricingLibrary.Computations;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.Strategies
{
    internal class VanillaNeutralStrategy : AbstractDeltaNeutralStrategy
    {

        public VanillaNeutralStrategy()
        {
            
        }

        public override Dictionary<string, double> UpdateCompo(IOption option, DataFeed market, List<DataFeed> dataFeedList, int NumberOfDaysPerYear)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            var Pricer = new Pricer();
            var volatility = 0.25;
            string UnderlyingShareId = option.UnderlyingShareIds[0];
            var S = decimal.ToDouble(market.PriceList[UnderlyingShareId]);
            PricingResults pricingResults = Pricer.Price((VanillaCall)option, market.Date, NumberOfDaysPerYear, S, volatility);
            double delta = pricingResults.Deltas[0];
            optionP = pricingResults.Price;
            res.Add(UnderlyingShareId, delta);

            return res;

        }

  
    }
}
