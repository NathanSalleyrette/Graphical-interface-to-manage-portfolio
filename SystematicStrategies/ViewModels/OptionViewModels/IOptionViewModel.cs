using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.ViewModels.DataViewModels
{
    internal interface IOptionViewModel
    {
        #region Public Properties

        IOption Option { get; }

        string Name { get; }

        #endregion Public Properties

        void Maj(string nom, Share[] actions, double[] weight, DateTime LastDate, double strike);

    }
}