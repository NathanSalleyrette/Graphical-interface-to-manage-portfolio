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

        public override Dictionary<string, double> UpdateCompo(IOption option, DataFeed market, List<DataFeed> dataFeedList, int NumberOfDaysPerYear)
        {
            //var volatility = !(dataFeedList is HistoricDataFeedProvider) ? 0.25 : null;
            Dictionary<string, double> res = new Dictionary<string, double>();
            var Pricer = new Pricer();
            var n = option.UnderlyingShareIds.Length;
            double[] spots = new double[n];
            double[] volatilities = new double[n];
            double[,] corMatrix = new double[n, n];
            for (var j = 0; j < n * n; j++) corMatrix[j % n, j / n] = 0.15;
            var i = 0;
            foreach (string UnderlyingShareId in option.UnderlyingShareIds)
            {
                volatilities[i] = 0.25;
                corMatrix[i, i] = 0.25;
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
