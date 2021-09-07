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
    internal class VanillaCallViewModel : IOptionViewModel
    {
        private VanillaCall option;
        private VanillaNeutralStrategy strategy;
        private PortfolioVanilla portfolio;

        public VanillaCallViewModel(string name, Share share, DateTime maturity, double strike)
        {
            option = new VanillaCall(name, share, maturity, strike);
            strategy = new VanillaNeutralStrategy();
            portfolio = new PortfolioVanilla();
        }

        public IOption Option => option;
        public IStrategy Strategy => strategy;
        public AbstractPortfolio Portfolio => portfolio;
        public string Name => "VanillaCall";
    }
}