using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.ViewModels.EstimationViewModels.cs
{
    internal class NoEstimationViewModel : IEstimationViewModel
    {
        private AbstractController controller;

        public NoEstimationViewModel()
        {
            controller = new Controller();
        }

        public AbstractController Controller
        {
            get { return controller; }
        }


        public string Name
        {
            get
            {
                return "No Estimation";
            }
        }
    }
}
