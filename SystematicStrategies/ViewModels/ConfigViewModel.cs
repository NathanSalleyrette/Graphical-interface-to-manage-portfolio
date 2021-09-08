using Newtonsoft.Json;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.Models;
using SystematicStrategies.Portfolio;
using SystematicStrategies.Strategies;

namespace SystematicStrategies.ViewModels
{
    internal class ConfigViewModel : BindableBase
    {
        private Config config;
        private string name;

        public ConfigViewModel(string file)
        {
            string text = System.IO.File.ReadAllText(file);
            config = JsonConvert.DeserializeObject<Config>(text);
            name = file;
        }

        public Config Config => config;
     public   string Name { get { return name; } }
    }
}