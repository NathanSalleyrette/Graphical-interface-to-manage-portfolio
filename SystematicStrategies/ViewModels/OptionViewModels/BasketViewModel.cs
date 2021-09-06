using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.ViewModels.DataViewModels
{
    internal class BasketViewModel : IOptionViewModel
    {
        private IOption option;

        public BasketViewModel()
        {
            option = new BasketOption("", new Share[0], new double[0], new DateTime(), 0);
        }

        public IOption Option
        {
            get { return option; }
        }

        public string Name
        {
            get { return "BasketOption"; }
        }
    }
}