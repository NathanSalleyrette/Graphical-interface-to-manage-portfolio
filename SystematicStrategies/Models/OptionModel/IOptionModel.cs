using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.Strategies;

namespace SystematicStrategies.Models.OptionModel
{
    class IOptionModel
    {
        #region Public Properties

        IOption Option { get; }

        IStrategy Strategy { get; }

        IPortfolio Portfolio { get; }

        #endregion Public Properties
    }
}
