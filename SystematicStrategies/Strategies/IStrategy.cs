﻿using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SystematicStrategies.Strategies
{
    internal interface IStrategy
    {
        double OptionPrice { get; set; }
        bool Rebalencing(DateTime t);
        //abstract public Dictionary<string, double> UpdateCompo(DateTime t, double value, DataFeed market);
        Dictionary<string, double> UpdateCompo(IOption option, DataFeed market, List<DataFeed> dataFeedList, int NumberOfDaysPerYear, double[] volatilities, double[,] corMatrix);

    }
}
