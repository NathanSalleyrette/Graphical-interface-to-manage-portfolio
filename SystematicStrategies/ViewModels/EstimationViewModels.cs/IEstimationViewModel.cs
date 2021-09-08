using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystematicStrategies.ViewModels.EstimationViewModels.cs
{
    internal interface IEstimationViewModel
    {
        #region Public Properties

        AbstractController Controller { get;  }

        string Name { get; }

        #endregion Public Properties
    }
}
