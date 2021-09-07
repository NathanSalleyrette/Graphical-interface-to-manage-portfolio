using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.Historique
{
    interface IDataManager
    {
        List<DataFeed> GetWindow(int numberOfDays, DataFeed[] globalMarket, DateTime end);
    }
}
