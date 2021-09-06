using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.Models.OptionModel
{
    class VanillaCallModel : IOptionModel
    {
        private VanillaCall option;
        private VanillaNeutralStrategy strategy;
        private VanillaPortfolio portfolio;

        public IOption Option => option;
        public IStrategy Strategy => strategy;
        public IPortfolio Portfolio => portfolio;

        public VanillaCallModel(string name, Share share, DateTime maturity, double strike)
        {
            option = new VanillaCall(name, share, maturity, strike);
            strategy = new VanillaNeutralStrategy();
            portfolio = new VanillaPortfolio();
        }
    }
}
