using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using SystematicStrategies.Portfolio;
using SystematicStrategies.Strategies;
using SystematicStrategies.ViewModels.DataViewModels;
using SystematicStrategies.Estimators;
using System.Linq;

namespace SystematicStrategies
{

    internal class ControllerHistoric : AbstractController
    {

        public override void CalculVolatilities()
        {
            var n = optionToHedge.UnderlyingShareIds.Length;
            var window = GetWindow(windowSize, dataFeedList, dayOfController);

            Estimator est = new Estimator();

            for (int i = 0; i < n; i++) {
                volatilities[i] = est.Volatity(window, dataFeedProvider.NumberOfDaysPerYear, optionToHedge.UnderlyingShareIds[i]);
            }
            if (n > 1) corMatrix = est.CovMatrix(window, optionToHedge.UnderlyingShareIds, dataFeedProvider.NumberOfDaysPerYear);
            else corMatrix = new double[,] { { volatilities[0]} };
            est.DispMatrix(corMatrix);
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
                return globalMarket.Take(indexOfDate+1).ToList();
            }
            else
            {
                return globalMarket.Skip(indexOfDate - numberOfDays).Take(numberOfDays+1).ToList();
            }
        }

        public override int WindowSizeSetter(int windowSize)
        {
            return windowSize;
        }


    }
}
