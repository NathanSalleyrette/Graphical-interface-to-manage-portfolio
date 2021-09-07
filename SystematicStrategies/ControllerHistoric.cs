using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using SystematicStrategies.Portfolio;
using SystematicStrategies.Strategies;
using SystematicStrategies.ViewModels.DataViewModels;
using SystematicStrategies.Estimators;
using SystematicStrategies.DataManager;
using SystematicStrategies.Historique;
using System.Linq;

namespace SystematicStrategies
{

    internal class ControllerHistoric : AbstractController
    {

        // A CHANGER
        public override void CalculVolatilities()
        {
            var n = optionToHedge.UnderlyingShareIds.Length;
            Estimator est = new Estimator();
            for (int i = 0; i < n; i++) {
                volatilities[i] =  est.Volatity(dataFeedList, dataFeedProvider.NumberOfDaysPerYear, optionToHedge.UnderlyingShareIds[i]);
            }
            var window = GetWindow(windowSize, dataFeedList, dayOfController);
            corMatrix =  est.CovMatrix(window, optionToHedge.UnderlyingShareIds, dataFeedProvider.NumberOfDaysPerYear);
        }

        public List<DataFeed> GetWindow(int numberOfDays, List<DataFeed> globalMarket, DateTime end)
        {
            DataFeed[] window = { };
            
            int indexOfDate = 0;

            while (indexOfDate < globalMarket.Count & globalMarket[indexOfDate].Date != end)
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
