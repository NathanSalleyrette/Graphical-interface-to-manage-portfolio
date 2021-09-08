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
    internal abstract class AbstractController
    {
        protected IOption optionToHedge;
        protected List<DataFeed> dataFeedList;
        protected IStrategy strategy;
        protected AbstractPortfolio portfolio;
        protected IDataFeedProvider dataFeedProvider;
        protected double payoff;
        public List<double> optionPrices;
        public List<double> portfolioValues;
        public string[] dateLabels;
        public double[] volatilities;
        public double[,] corMatrix;
        public int windowSize;
        public DateTime dayOfController;

        public void Initialize(IOptionViewModel option, DateTime startDate, DateTime endDate, IDataFeedProvider dataFeedProvider, int expectedWindowSize)
        {
            this.windowSize = WindowSizeSetter(expectedWindowSize);
            this.dataFeedProvider = dataFeedProvider;
            optionToHedge = option.Option;
            var n = optionToHedge.UnderlyingShareIds.Length;
            volatilities = new double[n];
            corMatrix = new double[n, n];

            dataFeedList = dataFeedProvider.GetDataFeed(option.Option.UnderlyingShareIds, startDate, endDate);
            dayOfController = dataFeedList[windowSize-1].Date;
            CalculVolatilities();

            dateLabels = new string[dataFeedList.Count - windowSize + 1];

            
            for (int i = 0; i < dataFeedList.Count - windowSize + 1; i++)
            {
                dateLabels[i] = dataFeedList[i + windowSize - 1].Date.ToString();
            }
            
            optionPrices = new List<double>() { };
            portfolioValues = new List<double>() { };
            this.strategy = option.Strategy;
            this.portfolio = option.Portfolio;
            portfolio.Initialize(optionToHedge, strategy, dataFeedList[0], dataFeedList, dataFeedProvider.NumberOfDaysPerYear, volatilities, corMatrix);
            optionPrices.Add(this.strategy.OptionPrice);
            portfolioValues.Add(portfolio.value);
        }

        public abstract void CalculVolatilities();

        public abstract int WindowSizeSetter(int windowSize);

        public void Start()
        {
            DateTime lastUpdate = dataFeedList[0].Date;
            DataFeed dataFeed;

            for (int i = windowSize; i < dataFeedList.Count; i++)
            {
                dataFeed = dataFeedList[i];
                dayOfController = dataFeed.Date;
                double dayCount = DayCount.CountBusinessDays(lastUpdate, dataFeed.Date);
                double riskRate = RiskFreeRateProvider.GetRiskFreeRateAccruedValue(dayCount / dataFeedProvider.NumberOfDaysPerYear);
                CalculVolatilities();
                portfolio.Update(optionToHedge, strategy, dataFeed, dataFeedList, dataFeedProvider.NumberOfDaysPerYear, riskRate, volatilities, corMatrix);
                lastUpdate = dataFeed.Date;
                payoff = optionToHedge.GetPayoff(dataFeed.PriceList);
                optionPrices.Add(strategy.OptionPrice);
                portfolioValues.Add(portfolio.value);
            }
        }

        public string ResultToString()
        {
            string result = "";
            result += "Valeur du portefeuille : " + portfolio.value + "\n"
                + "Payoff : " + payoff + "\n"
                + "Prix de l'option : " + strategy.OptionPrice + "\n";
            double trackingError = (portfolio.value - payoff) / optionPrices[0];
            result += "TrackingError : " + trackingError;
            return result;
        }
    }
}
