using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.Portfolio;
using SystematicStrategies.Strategies;

namespace SystematicStrategies.ViewModels.DataViewModels
{
    internal class BasketViewModel : IOptionViewModel
    {
        private BasketOption option;
        private BasketNeutralStrategy strategy;
        private PortfolioBasket portfolio;

        public BasketViewModel(string name, Share[] underlyingShares, double[] weights, DateTime maturity, double strike)
        {
            option = new BasketOption(name, underlyingShares, weights, maturity, strike);
            strategy = new BasketNeutralStrategy();
            portfolio = new PortfolioBasket();
        }

        public IOption Option => option;
        public IStrategy Strategy => strategy;
        public AbstractPortfolio Portfolio => portfolio;

        public string Name => "BasketOption";
    }
}