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
    internal class BasketNeutralStrategy : AbstractDeltaNeutralStrategy
    {

        public override Dictionary<string, double> UpdateCompo(IOption option, DataFeed market, List<DataFeed> dataFeedList, int NumberOfDaysPerYear, double[] volatilities, double[,] corMatrix)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            var Pricer = new Pricer();
            var n = option.UnderlyingShareIds.Length;
            double[] spots = new double[n];

            var i = 0;
            foreach (string UnderlyingShareId in option.UnderlyingShareIds)
            {

                spots[i] = decimal.ToDouble(market.PriceList[UnderlyingShareId]);
                i += 1;
            };
            PricingResults pricingResults = Pricer.Price((BasketOption)option, market.Date, NumberOfDaysPerYear, spots, volatilities, corMatrix);
            optionP = pricingResults.Price;
            i = 0;
            foreach (var delta in pricingResults.Deltas)
            {
                res.Add(option.UnderlyingShareIds[i], delta);
                i += 1;
            }
            return res;
        }
    }
}
