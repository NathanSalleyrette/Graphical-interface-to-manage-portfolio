using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.DataManager
{
    abstract class DataManager
    {
        public abstract DataFeed[] GetDataFeeds(DateTime start, DateTime end);
    }
}
