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
    internal class ControllerHistoric : AbstractController
    {


        // A CHANGER
        public override void CalculVolatilities()
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

    }
}
