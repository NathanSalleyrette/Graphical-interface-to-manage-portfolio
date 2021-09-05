using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SystematicStrategies.Strategies
{
    internal abstract class Strategy
    {
        public abstract double optionPrice { get; set; }
        abstract public bool Rebalencing(DateTime t);
        //abstract public Dictionary<string, double> UpdateCompo(DateTime t, double value, DataFeed market);
        abstract public Dictionary<string, double> UpdateCompo(Option option, DataFeed market, List<DataFeed> dataFeedList, int NumberOfDaysPerYear);

    }
}
