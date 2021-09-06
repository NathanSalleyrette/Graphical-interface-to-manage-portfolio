using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.Portfolio;
using SystematicStrategies.Strategies;

namespace SystematicStrategies.Models.OptionModel
{
    internal class VanillaCallModel : IOptionModel
    {
        private VanillaCall option;
        private VanillaNeutralStrategy strategy;
        private PortfolioVanilla portfolio;

        public IOption Option => option;
        public IStrategy Strategy => strategy;
        public AbstractPortfolio Portfolio => portfolio;

        public VanillaCallModel(string name, Share share, DateTime maturity, double strike)
        {
            option = new VanillaCall(name, share, maturity, strike);
            strategy = new VanillaNeutralStrategy();
            portfolio = new PortfolioVanilla();
        }
    }
}
