using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.DataManager
{
    class HistoricDataManager : DataManager
    {
        public override DataFeed[] GetDataFeeds(DateTime start, DateTime end)
        {
            DataFeed[] globalMarket = { };

            // Loading of the datas
            using (DataBaseAccessDataContext asdc = new DataBaseAccessDataContext())
            {
                var dates = (from lignes in asdc.HistoricalShareValues
                             select lignes.date).Distinct();

                // dates, tables 
                // Creation of a dataset
                foreach (var date in dates)
                {
                    var marketRecup = from lignes in asdc.HistoricalShareValues where lignes.date == date select new { lignes.id, lignes.value };
                    Dictionary<String, decimal> priceList = new Dictionary<string, decimal>();
                    foreach (var line in marketRecup)
                    {
                        priceList.Add(line.id, line.value);
                    }
                    DataFeed market = new DataFeed(date, priceList);
                    globalMarket.Append(market);
                }
            }
            return globalMarket;

        }

        public DataFeed[] GetWindow(int numberOfDays, DataFeed[] globalMarket, DateTime end)
        {
            DataFeed[] window = { };

            int indexOfDate = 0;

            while(indexOfDate < globalMarket.Length & globalMarket[indexOfDate].Date != end)
            {
                indexOfDate++;
            }

            if (indexOfDate < numberOfDays)
            {
                return (DataFeed[])globalMarket.Take(indexOfDate);
            }
            else
            {
                return (DataFeed[])globalMarket.Skip(indexOfDate - numberOfDays).Take(numberOfDays);
            }

            return window;
        }
    }
}
