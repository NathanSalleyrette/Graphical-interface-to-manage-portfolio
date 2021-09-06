using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.Models.OptionModel
{
    class BasketModel : IOptionModel
    {
        private BasketOption option;
        private BasketNeutralStrategy strategy;
        private BasketPortfolio portfolio;

        public IOption Option => option;
        public IStrategy Strategy => strategy;
        public IPortfolio Portfolio => portfolio;

        public BasketModel(string name, Share[] underlyingShares, double[] weights, DateTime maturity, double strike)
        {
            option = new BasketOption(name, underlyingShares, weights, maturity, strike);
            strategy = new BasketNeutralStrategy();
            portfolio = new BasketPortfolio();
        }
    }
}
