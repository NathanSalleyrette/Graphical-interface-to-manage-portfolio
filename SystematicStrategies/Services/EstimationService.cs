using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.ViewModels.EstimationViewModels.cs;

namespace SystematicStrategies.Services
{
    class EstimationService
    {
        public List<IEstimationViewModel> GetAvailableEstimation()
        {
            return new List<IEstimationViewModel>() { new EstimationViewModel(), new NoEstimationViewModel() };
        }
    }
}
