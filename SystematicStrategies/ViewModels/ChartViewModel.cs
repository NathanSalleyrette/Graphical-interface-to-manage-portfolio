using LiveCharts;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystematicStrategies.Models;

namespace SystematicStrategies.ViewModels
{
    class ChartViewModel : BindableBase
    {
        private SeriesCollection seriesCollection;
        private string[] labels;
        private Func<double, string> yFormatter;
        public ChartViewModel()
        {
            var underlyingChart = new Chart();
            seriesCollection = underlyingChart.SeriesCollection;
            labels = underlyingChart.Labels;
            yFormatter = underlyingChart.YFormatter;
        }

        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
           set
            {
                SetProperty(ref seriesCollection, value);
            }
        }

        public string[] Labels
        {
            get { return labels; }
            set
            {
                SetProperty(ref labels, value);
            }
        }

        public Func<double, string> YFormatter
        {
            get { return yFormatter; }
            set
            {
                SetProperty(ref yFormatter, value);
            }
        }

    }
}
