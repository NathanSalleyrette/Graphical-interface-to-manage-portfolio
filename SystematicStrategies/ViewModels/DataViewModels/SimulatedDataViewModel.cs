using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.ViewModels.DataViewModels
{
    internal class SimulatedDataViewModel : IDataViewModel
    {
        private IDataFeedProvider dataFeedProvider;
        private AbstractController controllerData;

        public SimulatedDataViewModel()
        {
            dataFeedProvider = new SimulatedDataFeedProvider();
            controllerData = new Controller();
        }

        public AbstractController ControllerData
        {
            get { return controllerData; }
        }

        public IDataFeedProvider DataFeedProvider
        {
            get { return dataFeedProvider; }
        }

        public string Name
        {
            get { return "Simulated"; }
        }
    }
}
