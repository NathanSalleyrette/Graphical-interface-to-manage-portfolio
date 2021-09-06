using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.DataManager
{
    class HistoricDataManager : IDataFeedProvider
    {
        public DateTime MinDate { get; }

        public string Name { get; }

        public DateTime MaxDate {get ; }

        public int NumberOfDaysPerYear => 252;

        public List<DataFeed> GetDataFeed(string[] ids, DateTime start, DateTime end)
        {
            List<DataFeed> globalMarket = new List<DataFeed>();

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
                        if (line.id.ElementAt(2).ToString() == " ")
                        {
                            priceList.Add(line.id.Substring(0, line.id.Length - 5), line.value);
                        }
                        else
                        {
                            priceList.Add(line.id.Substring(0, line.id.Length - 4), line.value);
                        }
                    }
                    DataFeed market = new DataFeed(date, priceList);
                    globalMarket.Add(market);
                }
                

                return globalMarket;
            }
            

        }

        public List<DataFeed> GetWindow(int numberOfDays, DataFeed[] globalMarket, DateTime end)
        {
            DataFeed[] window = { };

            int indexOfDate = 0;

            while (indexOfDate < globalMarket.Length & globalMarket[indexOfDate].Date != end)
            {
                indexOfDate++;
            }

            if (indexOfDate < numberOfDays)
            {
                return globalMarket.Take(indexOfDate).ToList();
            }
            else
            {
                return globalMarket.Skip(indexOfDate - numberOfDays).Take(numberOfDays).ToList();
            }
        }
    }
}