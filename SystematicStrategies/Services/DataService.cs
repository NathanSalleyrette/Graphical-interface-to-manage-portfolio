using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.ViewModels.DataViewModels;

namespace SystematicStrategies.Services
{
    class DataService
    {
        public Dictionary<string, IDataViewModel> GetAvailableDataFeedProvider()
        {
            return new Dictionary<string, IDataViewModel>() { { "SemiHistoricData", new SemiHistoricDataViewModel() }, { "SimulatedData", new SimulatedDataViewModel() }, { "HistoricData", new HistoricDataViewModel() } };
        }
    }
}
