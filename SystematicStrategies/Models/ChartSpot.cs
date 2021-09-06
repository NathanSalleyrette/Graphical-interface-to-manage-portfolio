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

            SeriesCollection = null;
            Labels = null;
            YFormatter = null;

        }

        //public void maj(List<double> optionPrices, List<double> portfolioValues, string[] dateLabels)
        //{
        //    Labels = dateLabels;
        //    SeriesCollection = new SeriesCollection
        //    {
        //        new LineSeries
        //        {
        //            Title = "Option",
        //            Values = new ChartValues<double>(optionPrices)

        //        },
        //        new LineSeries
        //        {
        //            Title = "Portfolio",
        //            Values = new ChartValues<double>(portfolioValues),
        //            PointGeometry = null
        //        }
        //    };
        //}
    }
}
