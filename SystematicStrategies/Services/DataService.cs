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
        public List<IDataViewModel> GetAvailableDataFeedProvider()
        {
            return new List<IDataViewModel>() { new SemiHistoricDataViewModel(), new SimulatedDataViewModel() };
        }
    }
}
