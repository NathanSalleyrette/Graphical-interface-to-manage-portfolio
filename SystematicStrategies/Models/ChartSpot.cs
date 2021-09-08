using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SystematicStrategies.Models
{
    internal class ChartSpot
    {

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public ChartSpot()
        {
            SeriesCollection = new SeriesCollection();
        }


    }
}
