using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.ViewModels.DataViewModels
{
    internal class VanillaCallViewModel : IOptionViewModel
    {
        private IOption option;

        public VanillaCallViewModel()
        {
            option = new VanillaCall("", new Share("",""), new DateTime(), 0);
        }

        public IOption Option
        {
            get { return option; }
        }

        public string Name
        {
            get { return "VanillaCall"; }
        }

        public void Maj(string nom, Share[] actions, double[] weight, DateTime LastDate, double strike)
        {
            option = new VanillaCall(nom, actions.First(), LastDate, strike);
        }

    }
}