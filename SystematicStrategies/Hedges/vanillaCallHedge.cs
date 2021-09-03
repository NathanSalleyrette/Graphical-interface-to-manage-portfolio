using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.Computations;
using PricingLibrary.Utilities;

namespace SystematicStrategies.Hedges
{
    class vanillaCallHedge
    {
        public double computeHedge(VanillaCall vanillaCall, IDataFeedProvider dataFeedProvider, DateTime startDate, DateTime endDate)
        {
            int nbDaysPerYear = dataFeedProvider.NumberOfDaysPerYear;
            string UnderlyingShareId = vanillaCall.UnderlyingShare.Id;
            List<DataFeed> dataFeedList = dataFeedProvider.GetDataFeed(new string[] { UnderlyingShareId }, startDate, endDate);
            var Pricer = new Pricer();
            var volatility = 0.25;
            //var volatility = ( HistoricDataFeedProvider.IsInstanceOf(dataFeedProvider) ? dataFeedList.volatility : 0.25 )
            var date = dataFeedList[0].Date;
            var S = decimal.ToDouble(dataFeedList[0].PriceList[UnderlyingShareId]);
            PricingResults pricingResults = Pricer.Price(vanillaCall, date, nbDaysPerYear, S, volatility);
            double V = pricingResults.Price;
            var delta = pricingResults.Deltas[0];
            var investmentFreeRiskRate = V - delta * S;
            dataFeedList.RemoveAt(0);
            foreach (DataFeed dataFeed in dataFeedList)
            {
                double dayCount = DayCount.CountBusinessDays(date, dataFeed.Date);
                double riskRate = RiskFreeRateProvider.GetRiskFreeRateAccruedValue(dayCount / nbDaysPerYear);
                S = decimal.ToDouble(dataFeed.PriceList[UnderlyingShareId]);
                pricingResults = Pricer.Price(vanillaCall, date, nbDaysPerYear, S, volatility);
                V = investmentFreeRiskRate * riskRate + delta * S;
                date = dataFeed.Date;
                delta = pricingResults.Deltas[0];
                investmentFreeRiskRate = V - delta * S;


            }
            var resultFinal = V - Math.Max(S - vanillaCall.Strike, 0);
            var trackingError = resultFinal / pricingResults.Price;
            return trackingError;
        }
    }
}
