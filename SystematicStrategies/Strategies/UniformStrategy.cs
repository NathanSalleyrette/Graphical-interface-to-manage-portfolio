//using PricingLibrary.Utilities.MarketDataFeed;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SystematicStrategies.Strategies
//{
//    class UniformStrategy : Strategy
//    {
//        public override bool Rebalencing(DateTime t)
//        {
//            return t.DayOfWeek == DayOfWeek.Monday;
//        }

//        public override Dictionary<string, double> UpdateCompo(DateTime t, double value, DataFeed market)
//        {
//            Dictionary<string, double> q = new Dictionary<string, double>();

//            foreach (var asset in market.PriceList.Keys)
//            {
//                q[asset] = value / (market.PriceList.Count() * (double)(market.PriceList[asset]));
//            }


//            return q;
//        }
//    }
//}
