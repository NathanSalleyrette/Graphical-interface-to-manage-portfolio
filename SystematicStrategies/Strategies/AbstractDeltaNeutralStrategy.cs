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
    internal abstract class AbstractDeltaNeutralStrategy : IStrategy
    {
        protected double optionP;
        public double OptionPrice
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

        public bool Rebalencing(DateTime t)
        {
            return t.DayOfWeek == DayOfWeek.Monday;
        }

        public abstract Dictionary<string, double> UpdateCompo(IOption option, DataFeed market, List<DataFeed> dataFeedList, int NumberOfDaysPerYear, double[] volatilities, double[,] corMatrix);

    }


}
