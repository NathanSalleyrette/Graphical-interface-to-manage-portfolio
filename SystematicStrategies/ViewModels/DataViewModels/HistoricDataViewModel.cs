using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.DataManager;

namespace SystematicStrategies.ViewModels.DataViewModels
{
    internal class HistoricDataViewModel : IDataViewModel
    {
        private IDataFeedProvider dataFeedProvider;
        private AbstractController controllerData;

        public HistoricDataViewModel()
        {
            dataFeedProvider = new HistoricDataManager();
            controllerData = new ControllerHistoric();

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
            get { return "Historic"; }
        }
    }
}
