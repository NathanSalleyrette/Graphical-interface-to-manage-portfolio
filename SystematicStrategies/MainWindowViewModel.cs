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
        private DateTime firstDate;
        private DateTime lastDate;
        private DateTime firstDateDisplay;
        private DateTime lastDateDisplay;

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(StartController, CanStartController);
            ResetCommand = new DelegateCommand(ResetController, CanStopController);
            SaveConfigCommand = new DelegateCommand(SaveConfigController, CanStartController);
            var dataService = new DataService();
            var AvailableDataFeedProviderDic = dataService.GetAvailableDataFeedProvider();
            AvailableDataFeedProvider = AvailableDataFeedProviderDic.Values.ToList();
            ChartVM = new ChartViewModel();
            dataVM = AvailableDataFeedProvider.First();
            string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "SystematicStrategies/Configs/config.json");
            string text = System.IO.File.ReadAllText(startupPath);
            Console.WriteLine(text);
            config = JsonConvert.DeserializeObject<Config>(text);
            FirstDate = config.startDate;
            LastDate = config.maturity;
            FirstDateDisplay = config.startDate;
            LastDateDisplay = config.maturity;
            dataVM = AvailableDataFeedProviderDic[config.dataType];
            ControllerStarted = false;
        }

        public List<IDataViewModel> AvailableDataFeedProvider { get; }

        public List<IOptionViewModel> AvailableOptions { get; }

        public DateTime FirstDate { 
            get { return firstDate; }
            set
            {
                if (VerifyDate(value)) SetProperty(ref firstDate, value);
                else SetProperty(ref firstDate, FirstDate);
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime LastDate { 
            get { return lastDate; }
            set
            {
                if (VerifyDate(value)) SetProperty(ref lastDate, value);
                else SetProperty(ref lastDate, LastDate);
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime FirstDateDisplay
        {
            get { return firstDateDisplay; }
            set
            {
                SetProperty(ref firstDateDisplay, value);
            }
        }

        public DateTime LastDateDisplay
        {
            get { return lastDateDisplay; }
            set
            {
                SetProperty(ref lastDateDisplay, value);
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

        public DelegateCommand SaveConfigCommand { get; private set; }

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

        private void SaveConfigController()
        {

        }

        private bool CanStartController()
        {
            return !controllerStarted & FirstDate < LastDate;
        }

        private bool CanStopController()
        {
            return controllerStarted;
        }


        private bool VerifyDate(DateTime date)
        {
            DateTime startDate = new DateTime(2009, 12, 14);
            DateTime endDate = new DateTime(2013, 06, 13);
            return (date.DayOfWeek != DayOfWeek.Sunday) & (date.DayOfWeek != DayOfWeek.Saturday) & (date >= startDate) & (date <= endDate);
        }



    }

}
