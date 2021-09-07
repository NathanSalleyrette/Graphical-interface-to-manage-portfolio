using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using SystematicStrategies.Portfolio;
using SystematicStrategies.Strategies;
using SystematicStrategies.ViewModels.DataViewModels;
using SystematicStrategies.Estimators;
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

            corMatrix =  est.CovMatrix(dataFeedList, optionToHedge.UnderlyingShareIds, dataFeedProvider.NumberOfDaysPerYear);
        }

    }
}
