using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.ViewModels.DataViewModels;

namespace SystematicStrategies.Services
{
    class OptionService
    {
        public List<IOptionViewModel> GetAvailableOptions()
        {
            return new List<IOptionViewModel>() { new VanillaCallViewModel(), new BasketViewModel()};
        }
    }
}
