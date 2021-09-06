﻿using LiveCharts;
using LiveCharts.Wpf;
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

        public void reset()
        {
            foreach(var collection in SeriesCollection)
            {
                SeriesCollection.Remove(collection);
            }
            Labels = new string[] { };
            YFormatter = null;
        }

        public void maj(List<double> optionPrices, List<double> portfolioValues, string[] dateLabels)
        {
            Labels = dateLabels;
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double>(optionPrices)
               
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double>(portfolioValues),
                    PointGeometry = null
                }
            };
        }
    }
}