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
    internal interface IOptionViewModel
    {
        #region Public Properties

        IOption Option { get;}
        IStrategy Strategy { get; }
        AbstractPortfolio Portfolio { get; }

        string Name { get; }

        #endregion Public Properties

    }
}