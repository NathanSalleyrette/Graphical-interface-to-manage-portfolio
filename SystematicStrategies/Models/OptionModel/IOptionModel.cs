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
    internal interface IOptionModel
    {
        #region Public Properties

        IOption Option { get; }

        IStrategy Strategy { get; }

        AbstractPortfolio Portfolio { get; }

        #endregion Public Properties
    }
}
