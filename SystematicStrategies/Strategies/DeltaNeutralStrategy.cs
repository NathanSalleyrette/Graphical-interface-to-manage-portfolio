//using PricingLibrary.Utilities.MarketDataFeed;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using PricingLibrary.FinancialProducts;
//using PricingLibrary.Computations;
//using PricingLibrary.Utilities;

//namespace SystematicStrategies.Strategies
//{
//    class DeltaNeutralStrategy : Strategy
//    {
//        private double optionP;
//        public override double optionPrice
//        {
//            get
//            {
//                return optionP;
//            }
//            set
//            {
//                optionP = value;
//            }
//        }

//        public override bool Rebalencing(DateTime t)
//        {
//            return t.DayOfWeek == DayOfWeek.Monday;
//        }

//        public override Dictionary<string, double> UpdateCompo(Option option, DataFeed market, List<DataFeed> dataFeedList, int NumberOfDaysPerYear)
//        {
//            //var volatility = !(dataFeedList is HistoricDataFeedProvider) ? 0.25 : null;
//            Dictionary<string, double> res = new Dictionary<string, double>();
//            var Pricer = new Pricer();
//            if (option is VanillaCall)
//            {
//                var volatility = 0.25;
//                string UnderlyingShareId = option.UnderlyingShareIds[0];
//                var S = decimal.ToDouble(market.PriceList[UnderlyingShareId]);
//                PricingResults pricingResults = Pricer.Price((VanillaCall)option, market.Date, NumberOfDaysPerYear, S, volatility) ;
//                double delta = pricingResults.Deltas[0];
//                optionP = pricingResults.Price;
//                res.Add(UnderlyingShareId, delta);

//            }
//            else
//            {
//                var n = option.UnderlyingShareIds.Length;
//                double[] spots = new double[n];
//                double[] volatilities = new double[n];
//                double[,] corMatrix = new double[n, n];
//                for (var j = 0; j < n * n; j++) corMatrix[j % n, j / n] = 0.15;
//                var i = 0;
//                foreach (string UnderlyingShareId in option.UnderlyingShareIds)
//                {
//                    volatilities[i] = 0.25;
//                    corMatrix[i,i] = 0.25;
//                    spots[i] = decimal.ToDouble(market.PriceList[UnderlyingShareId]);
//                    i += 1;
//                };
//                PricingResults pricingResults = Pricer.Price((BasketOption)option, market.Date, NumberOfDaysPerYear, spots, volatilities, corMatrix);
//                optionP = pricingResults.Price;
//                i = 0;
//                foreach (var delta in pricingResults.Deltas)
//                {
//                    res.Add(option.UnderlyingShareIds[i], delta);
//                    i += 1;
//                }
//            }
//            return res;
//        }
//    } 
//}
