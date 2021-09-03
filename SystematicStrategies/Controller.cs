using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.Strategies;

namespace SystematicStrategies
{
    class Controller
    {
        Option optionToHedge;
        List<DataFeed> dataFeedList;
        Strategy strategy;
        Portofolio portofolio;
        IDataFeedProvider dataFeedProvider;

        public Controller(Option option, DateTime startDate, DateTime endDate, IDataFeedProvider dataFeedProvider, Strategy strategy)
        {
            this.dataFeedProvider = dataFeedProvider;
            optionToHedge = option;
            dataFeedList = dataFeedProvider.GetDataFeed(option.UnderlyingShareIds, startDate, endDate);
            this.strategy = strategy;
            portofolio = new Portofolio(optionToHedge, this.strategy, dataFeedList[0], dataFeedList, dataFeedProvider.NumberOfDaysPerYear);

        }
        public void start()
        {
            DateTime lastUpdate = dataFeedList[0].Date;
            foreach (DataFeed dataFeed in dataFeedList)
            {
                double dayCount = DayCount.CountBusinessDays(lastUpdate, dataFeed.Date);
                double riskRate = RiskFreeRateProvider.GetRiskFreeRateAccruedValue(dayCount / dataFeedProvider.NumberOfDaysPerYear);
                portofolio.update(optionToHedge, strategy, dataFeed, dataFeedList, dataFeedProvider.NumberOfDaysPerYear, riskRate);
            }
        }
    }
}
