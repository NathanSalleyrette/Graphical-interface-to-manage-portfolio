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
        private string errorMessage = "";
        private string infos = "";
        private IDataViewModel dataVM;
        private IOptionViewModel optionVM;
        private Config configVM;
        private DateTime startDate;
        private DateTime endDate;
        private Dictionary<string, IDataViewModel> AvailableDataFeedProviderDic;

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(StartController, CanStartController);
            ResetCommand = new DelegateCommand(ResetController, CanStopController);
            SaveConfigCommand = new DelegateCommand(SaveConfigController, CanStartController);
            var dataService = new DataService();
            AvailableDataFeedProviderDic = dataService.GetAvailableDataFeedProvider();
            AvailableDataFeedProvider = AvailableDataFeedProviderDic.Values.ToList();
            var configService = new ConfigService();
            AvailableConfigs = configService.GetAvailableConfigs();
            ChartVM = new ChartViewModel();
            dataVM = AvailableDataFeedProvider.First();
            configVM = AvailableConfigs.First();
            StartDate = configVM.startDate;
            EndDate = configVM.maturity;
            dataVM = AvailableDataFeedProviderDic[configVM.dataType];
            infos = configInfos(configVM);
            ControllerStarted = false;
        }

        public List<IDataViewModel> AvailableDataFeedProvider { get; }

        public List<Config> AvailableConfigs { get; }

        public List<IOptionViewModel> AvailableOptions { get; }

        public DateTime StartDate { 
            get { return startDate; }
            set
            {
                SetProperty(ref startDate, value);
                StartCommand.RaiseCanExecuteChanged();
                SaveConfigCommand.RaiseCanExecuteChanged();

            }
        }

        public DateTime EndDate { 
            get { return endDate; }
            set
            {
                SetProperty(ref endDate, value);
                StartCommand.RaiseCanExecuteChanged();
                SaveConfigCommand.RaiseCanExecuteChanged();
            }
        }

        /*public DateTime StartDateDisplay
        {
            get { return startDateDisplay; }
            set
            {
                SetProperty(ref startDateDisplay, value);
            }
        }

        public DateTime EndDateDisplay
        {
            get { return endDateDisplay; }
            set
            {
                SetProperty(ref endDateDisplay, value);
            }
        }*/

        public IDataViewModel DataVM
        {
            get { return dataVM; }
            set
            {
                SetProperty(ref dataVM, value);
                StartCommand.RaiseCanExecuteChanged();
                SaveConfigCommand.RaiseCanExecuteChanged();
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

        public Config ConfigVM
        {
            get { return configVM; }
            set
            {
                SetProperty(ref configVM, value);
                Infos = configInfos(configVM);
                StartDate = value.startDate;
                EndDate = value.maturity;
                DataVM = AvailableDataFeedProviderDic[value.dataType];

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

        public string Infos
        {
            get
            {
                return infos;
            }

            set
            {
                SetProperty(ref infos, value);
            }

        }
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                if (errorMessage != value)
                {
                    SetProperty(ref errorMessage, value);
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
                SaveConfigCommand.RaiseCanExecuteChanged();
                ResetCommand.RaiseCanExecuteChanged();
            }
        }



        private void StartController()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().First(t => t.Name == (configVM.type + "ViewModel"));
            optionVM = (IOptionViewModel)Activator.CreateInstance(type, new object[5] { configVM.name, configVM.underlyingShares, configVM.weights, EndDate, configVM.strike });
            controller = dataVM.ControllerData;
            controller.Initialize(optionVM, startDate, endDate, dataVM.DataFeedProvider, 3);
            controller.Start();
            ControllerStarted = true;
            Result = controller.ResultToString();
            Result += "\n" + "Date du début : " + startDate.ToString();
            Result += "\n" + "Date de fin : " + endDate.ToString();
            ChartVM.Maj(controller.optionPrices, controller.portfolioValues, controller.dateLabels);

        }

        private void ResetController()
        {
            ChartVM.Reset();
            ControllerStarted = false;
            Result = "Résultat en attente";
        }

        private void SaveConfigController()
        {
            string json = JsonConvert.SerializeObject(configVM);
            Console.Write(json);
            File.WriteAllText(@"Configs\path.json", json);

        }

        private bool CanStartController()
        {
            return !controllerStarted & VerifyDate();
        }

        private bool CanStopController()
        {
            return controllerStarted;
        }


        private bool VerifyDate()
        {
            DateTime firstDate = new DateTime(2009, 12, 14);
            DateTime lastDate = new DateTime(2013, 06, 13);
            DateTime firstDateHistoric = new DateTime(2010, 01, 01);
            DateTime lastDateHistoric = new DateTime(2015, 08, 20);
            List<System.DayOfWeek> weekEndDays = new List<System.DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Saturday };
            if(!weekEndDays.Contains(StartDate.DayOfWeek) & !weekEndDays.Contains(EndDate.DayOfWeek) & ((StartDate >= firstDate & EndDate <= lastDate & configVM.dataType == "SemiHistoricData") | (StartDate >= firstDateHistoric & EndDate <= lastDateHistoric & configVM.dataType == "HistoricData") | configVM.dataType == "SimulatedData" ) & StartDate < EndDate)
            {
                ErrorMessage = "";
                return true;
            }
            else
            {
                ErrorMessage = "Erreur: Dates choisies invalides";
                return false;
            }
        }

        private string configInfos(Config config)
        {
            string res = "Infos sur l'option :\n";
            res += "Strike : " + config.strike + "\n";
            res += "Actions sous-jacentes : ";
            var i = 0;
            foreach(Share share in config.underlyingShares)
            {
                res += share.Name + " (" + config.weights[i] + ") / ";
            }
            return res;
        }
    }
}
