using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.ViewModels.DataViewModels
{
    internal interface IDataViewModel
    {
        #region Public Properties

        IDataFeedProvider DataFeedProvider { get; }

        string Name { get; }

        #endregion Public Properties
    }
}
