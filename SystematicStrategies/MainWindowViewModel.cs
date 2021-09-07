using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Controls;
using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using SystematicStrategies.Strategies;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SystematicStrategies.ViewModels.DataViewModels;
using SystematicStrategies.Services;
using SystematicStrategies.Models;
using SystematicStrategies.ViewModels;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace SystematicStrategies
{
    internal class MainWindowViewModel : BindableBase
    {
        private AbstractController controller;
        private bool controllerStarted;
        private string _result = "Résultat en attente";
        private IDataViewModel dataVM;
        private IOptionViewModel optionVM;
        private Config config = null;
        public MainWindowViewModel()
        {
            //FirstDate = new DatePicker();
            //FirstDate.SelectedDate = new DateTime(2009, 12, 12);
            StartCommand = new DelegateCommand(StartController, CanStartController);
            ResetCommand = new DelegateCommand(ResetController, CanStopController);
            var dataService = new DataService();
            var AvailableDataFeedProviderDic = dataService.GetAvailableDataFeedProvider();
            AvailableDataFeedProvider = AvailableDataFeedProviderDic.Values.ToList();
            ChartVM = new ChartViewModel();
            dataVM = AvailableDataFeedProvider.First();
            string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "SystematicStrategies/Configs/config.json");
            string text = System.IO.File.ReadAllText(startupPath);
            Console.WriteLine(text);
            config = JsonConvert.DeserializeObject<Config>(text);

            dataVM = AvailableDataFeedProviderDic[config.dataType];
        }

        public List<IDataViewModel> AvailableDataFeedProvider { get; }

        public List<IOptionViewModel> AvailableOptions { get; }

        public DateTime FirstDate { 
            get { return config.startDate; }
            set
            {
                SetProperty(ref config.startDate, value);
            }
        }

        public DateTime LastDate { 
            get { return config.maturity; }
            set
            {
                SetProperty(ref config.maturity, value);
            }
        }

        public IDataViewModel DataVM
        {
            get { return dataVM; }
            set
            {
                SetProperty(ref dataVM, value);
            }
        }

        public IOptionViewModel OptionVM
        {
            get { return optionVM; }
            set
            {
                SetProperty(ref optionVM, value);
            }
        }

        public ChartViewModel ChartVM { get; }

        public string Result
        {
            get
            {
                return _result;
            }

            set
            {
                if (_result != value)
                {
                    SetProperty(ref _result, value);
                }

            }

        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged(string propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        public DelegateCommand StartCommand { get; private set; }

        public DelegateCommand ResetCommand { get; private set; }

        public bool ControllerStarted
        {
            get { return controllerStarted; }
            set
            {
                SetProperty(ref controllerStarted, value);
                StartCommand.RaiseCanExecuteChanged();
                ResetCommand.RaiseCanExecuteChanged();
            }
        }



        private void StartController()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().First(t => t.Name == config.type);
            optionVM = (IOptionViewModel)Activator.CreateInstance(type, new object[5] { config.name, config.underlyingShares, config.weights, config.maturity, config.strike });
            controller = dataVM.ControllerData;
            controller.Initialize(optionVM, FirstDate, LastDate, dataVM.DataFeedProvider);
            controller.start();
            ControllerStarted = true;
            Result = controller.ResultToString();
            Result += "\n" + "Date du début : " + FirstDate.ToString();
            Result += "\n" + "Date de fin : " + LastDate.ToString();
            ChartVM.maj(controller.optionPrices, controller.portfolioValues, controller.dateLabels);
        }

        private void ResetController()
        {
            ChartVM.reset();
            ControllerStarted = false;
            Result = "Résultat en attente";
        }

        private bool CanStartController()
        {
            return !controllerStarted;
        }

        private bool CanStopController()
        {
            return controllerStarted;
        }



    }

}
