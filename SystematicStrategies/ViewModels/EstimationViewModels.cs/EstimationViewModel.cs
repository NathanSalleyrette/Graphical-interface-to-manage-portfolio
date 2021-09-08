using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.ViewModels.EstimationViewModels.cs
{
    internal class EstimationViewModel : IEstimationViewModel
    {
        private AbstractController controller;

        public EstimationViewModel()
        {
            controller = new ControllerHistoricViewModel();
        }

        public AbstractController Controller
        {
            get { return controller; }
        }


        public string Name
        {
            get
            {
                return "Estimate";
            }
        }
    }
}
