using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
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
        double payoff;
        List<double> optionPrices;
        List<double> portfolioValues;

        public Controller(Option option, DateTime startDate, DateTime endDate, IDataFeedProvider dataFeedProvider, Strategy strategy)
        {
            this.dataFeedProvider = dataFeedProvider;
            optionToHedge = option;
            dataFeedList = dataFeedProvider.GetDataFeed(option.UnderlyingShareIds, startDate, endDate);
            optionPrices = new List<double>() { };
            portfolioValues = new List<double>() { };
            this.strategy = strategy;
            portofolio = new Portofolio(optionToHedge, this.strategy, dataFeedList[0], dataFeedList, dataFeedProvider.NumberOfDaysPerYear);
            optionPrices.Add(this.strategy.optionPrice);
            portfolioValues.Add(portofolio.value);
        }
        public void start()
        {
            DateTime lastUpdate = dataFeedList[0].Date;
            foreach (DataFeed dataFeed in dataFeedList)
            {
                double dayCount = DayCount.CountBusinessDays(lastUpdate, dataFeed.Date);
                double riskRate = RiskFreeRateProvider.GetRiskFreeRateAccruedValue(dayCount / dataFeedProvider.NumberOfDaysPerYear);
                portofolio.update(optionToHedge, strategy, dataFeed, dataFeedList, dataFeedProvider.NumberOfDaysPerYear, riskRate);
                lastUpdate = dataFeed.Date;
                payoff = optionToHedge.GetPayoff(dataFeed.PriceList);
                optionPrices.Add(strategy.optionPrice);
                portfolioValues.Add(portofolio.value);
            }
            Console.WriteLine(portofolio.value);
            Console.WriteLine(payoff);
            Console.WriteLine(strategy.optionPrice);
            Console.WriteLine(optionPrices[0]);
            double trackingError = (portofolio.value - payoff) / optionPrices[0];
            Console.WriteLine(trackingError);
        }

        public string ResultToString()
        {
            string result = "";
            result += "Valeur du portefeuille : " + portofolio.value + "\n" 
                + "Payoff : " + payoff + "\n"
                + "Prix de l'option : " + strategy.optionPrice + "\n";
            double trackingError = (portofolio.value - payoff) / optionPrices[0];
            result += "TrackingError : " + trackingError;
            return result;
        }
    }
}
