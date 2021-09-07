using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using SystematicStrategies.Portfolio;
using SystematicStrategies.Strategies;
using SystematicStrategies.ViewModels.DataViewModels;

namespace SystematicStrategies
{
    class Controller
    {
        IOption optionToHedge;
        List<DataFeed> dataFeedList;
        IStrategy strategy;
        AbstractPortfolio portfolio;
        IDataFeedProvider dataFeedProvider;
        double payoff;
        public List<double> optionPrices;
        public List<double> portfolioValues;
        public string[] dateLabels;
        public double[] volatilities;
        public double[,] corMatrix;

        public Controller(IOptionViewModel option, DateTime startDate, DateTime endDate, IDataFeedProvider dataFeedProvider)
        {
            this.dataFeedProvider = dataFeedProvider;
            optionToHedge = option.Option;
            var n = optionToHedge.UnderlyingShareIds.Length;
            volatilities = new double[n];
            corMatrix = new double[n, n];
            CalculVolatilities();

            dataFeedList = dataFeedProvider.GetDataFeed(option.Option.UnderlyingShareIds, startDate, endDate);
            dateLabels = new string[dataFeedList.Count];
            var i = 0;
            foreach(var dataFeed in dataFeedList)
            {
                dateLabels[i] = dataFeed.Date.ToString();
                i += 1;
            }
            optionPrices = new List<double>() { };
            portfolioValues = new List<double>() { };
            this.strategy = option.Strategy;
            this.portfolio = option.Portfolio;
            portfolio.Initialize(optionToHedge, strategy, dataFeedList[0], dataFeedList, dataFeedProvider.NumberOfDaysPerYear, volatilities, corMatrix);
            optionPrices.Add(this.strategy.optionPrice);
            portfolioValues.Add(portfolio.value);
        }

        public void CalculVolatilities()
        {
            var n = optionToHedge.UnderlyingShareIds.Length;
            for (var j = 0; j < n * n; j++) corMatrix[j % n, j / n] = 0.15;
            var i = 0;
            foreach (string UnderlyingShareId in optionToHedge.UnderlyingShareIds)
            {
                volatilities[i] = 0.25;
                corMatrix[i, i] = 0.25;
                i += 1;
            };
        }
        public void start()
        {
            DateTime lastUpdate = dataFeedList[0].Date;
            foreach (DataFeed dataFeed in dataFeedList)
            {
                double dayCount = DayCount.CountBusinessDays(lastUpdate, dataFeed.Date);
                double riskRate = RiskFreeRateProvider.GetRiskFreeRateAccruedValue(dayCount / dataFeedProvider.NumberOfDaysPerYear);
                //CalculVolatilities();
                portfolio.Update(optionToHedge, strategy, dataFeed, dataFeedList, dataFeedProvider.NumberOfDaysPerYear, riskRate, volatilities, corMatrix);
                lastUpdate = dataFeed.Date;
                payoff = optionToHedge.GetPayoff(dataFeed.PriceList);
                optionPrices.Add(strategy.optionPrice);
                portfolioValues.Add(portfolio.value);
            }
            Console.WriteLine(portfolio.value);
            Console.WriteLine(payoff);
            Console.WriteLine(strategy.optionPrice);
            Console.WriteLine(optionPrices[0]);
            double trackingError = (portfolio.value - payoff) / optionPrices[0];
            Console.WriteLine(trackingError);
        }

        public string ResultToString()
        {
            string result = "";
            result += "Valeur du portefeuille : " + portfolio.value + "\n" 
                + "Payoff : " + payoff + "\n"
                + "Prix de l'option : " + strategy.optionPrice + "\n";
            double trackingError = (portfolio.value - payoff) / optionPrices[0];
            result += "TrackingError : " + trackingError;
            return result;
        }
    }
}
