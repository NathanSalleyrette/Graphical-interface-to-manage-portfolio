using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Computations;
using PricingLibrary.Utilities;

namespace SystematicStrategies.Strategies
{
    class DeltaNeutralStrategy : Strategy
    {
        private double optionP;
        public override double optionPrice
        {
            get
            {
                return optionP;
            }
            set
            {
                optionP = value;
            }
        }

        public override bool Rebalencing(DateTime t)
        {
            return t.DayOfWeek == DayOfWeek.Monday;
        }

        public override Dictionary<string, double> UpdateCompo(Option option, DataFeed market, List<DataFeed> dataFeedList, int NumberOfDaysPerYear)
        {
            //var volatility = !(dataFeedList is HistoricDataFeedProvider) ? 0.25 : null;
            var volatility = 0.25;
            var Pricer = new Pricer();
            string UnderlyingShareId = option.UnderlyingShareIds[0];
            var S = decimal.ToDouble(market.PriceList[UnderlyingShareId]);
            PricingResults pricingResults = Pricer.Price((VanillaCall) option, market.Date, NumberOfDaysPerYear, S, volatility);
            double delta = pricingResults.Deltas[0];
            optionP = pricingResults.Price;
            Dictionary<string, double> res = new Dictionary<string, double>();
            res.Add(UnderlyingShareId, delta);
            return res;
        }
    }

    
}
