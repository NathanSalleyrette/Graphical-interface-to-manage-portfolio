using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using Prism.Commands;
using PricingLibrary.FinancialProducts;
using SystematicStrategies.ViewModels.DataViewModels;
using SystematicStrategies.Services;
using SystematicStrategies.Models;
using SystematicStrategies.ViewModels;
using SystematicStrategies.ViewModels.EstimationViewModels.cs;
using PricingLibrary.Utilities;

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
        private IEstimationViewModel controllerVM;
        private int windowSizeVM;
        private int rebalancingRateVM;
        private Config configVM;
        private DateTime startDate;
        private DateTime endDate;
        private Dictionary<string, IDataViewModel> AvailableDataFeedProviderDic;

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(StartController, CanStartController);
            ResetCommand = new DelegateCommand(ResetController, CanStopController);
            var dataService = new DataService();
            var estimationService = new EstimationService();
            AvailableDataFeedProviderDic = dataService.GetAvailableDataFeedProvider();
            AvailableDataFeedProvider = AvailableDataFeedProviderDic.Values.ToList();
            AvailableEstimationProvider = estimationService.GetAvailableEstimation();
            var configService = new ConfigService();
            AvailableConfigs = configService.GetAvailableConfigs();
            ChartVM = new ChartViewModel();
            dataVM = AvailableDataFeedProvider.First();
            configVM = AvailableConfigs.First();
            controllerVM = configVM.isEstimated? AvailableEstimationProvider.First() : AvailableEstimationProvider.Last();
            windowSizeVM = configVM.estimatedWindowSize;
            rebalancingRateVM = configVM.rebalancingRate;
            StartDate = configVM.startDate;
            EndDate = configVM.maturity;
            dataVM = AvailableDataFeedProviderDic[configVM.dataType];
            configInfos();
            ControllerStarted = false;
        }

        public List<IEstimationViewModel> AvailableEstimationProvider { get; }
        public List<IDataViewModel> AvailableDataFeedProvider { get; }

        public List<Config> AvailableConfigs { get; }

        public List<IOptionViewModel> AvailableOptions { get; }

        public DateTime StartDate { 
            get { return startDate; }
            set
            {
                SetProperty(ref startDate, value);
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime EndDate { 
            get { return endDate; }
            set
            {
                SetProperty(ref endDate, value);
                configVM.maturity = endDate;
                configInfos();
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        public IEstimationViewModel ControllerVM
        {
            get { return controllerVM; }
            set
            {
                SetProperty(ref controllerVM, value);
                configVM.isEstimated = controllerVM is EstimationViewModel;
                configInfos();
            }
        }

        public IDataViewModel DataVM
        {
            get { return dataVM; }
            set
            {
                SetProperty(ref dataVM, value);
                if (dataVM is HistoricDataViewModel)
                {
                    ControllerVM = AvailableEstimationProvider.First();
                }
                configInfos();
                StartCommand.RaiseCanExecuteChanged();
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

        public int WindowSizeVM
        {
            get { return windowSizeVM; }
            set
            {
                SetProperty(ref windowSizeVM, value);
                configVM.estimatedWindowSize = windowSizeVM;
                configInfos();
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        public int RebalancingRateVM
        {
            get { return rebalancingRateVM; }
            set
            {
                SetProperty(ref rebalancingRateVM, value);
                configVM.rebalancingRate = rebalancingRateVM;
                configInfos();
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        public Config ConfigVM
        {
            get { return configVM; }
            set
            {
                SetProperty(ref configVM, value);
                configInfos();
                ControllerVM = configVM.isEstimated ? AvailableEstimationProvider.First() : AvailableEstimationProvider.Last();
                WindowSizeVM = configVM.estimatedWindowSize;
                RebalancingRateVM = configVM.rebalancingRate;
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
            var type = assembly.GetTypes().First(t => t.Name == (configVM.type + "ViewModel"));
            optionVM = (IOptionViewModel)Activator.CreateInstance(type, new object[5] { configVM.name, configVM.underlyingShares, configVM.weights, EndDate, configVM.strike });
            controller = controllerVM.Controller;
            try
            {
                controller.Initialize(optionVM, startDate, endDate, dataVM.DataFeedProvider, windowSizeVM, rebalancingRateVM);
                controller.Start();
           
                ControllerStarted = true;
                Result = controller.ResultToString();
                Result += "\n" + "Date du début : " + startDate.ToString();
                Result += "\n" + "Date de fin : " + endDate.ToString();
                if (controller.portfolioValues.Count < 10000)
                {
                    ChartVM.Maj(controller.optionPrices, controller.portfolioValues, controller.dateLabels);
                }
                else {
                    ErrorMessage = "Soucis: Taille du jeu de données trop élevée pour afficher un graphe";
                }
            }
            catch (WarningWREException e)
            {
                ErrorMessage = "La taille de la fenêtre ne permet pas de calculer correctement la matrice de corrélation, veuillez choisir une fenêtre plus grande";
            }

        }

        private void ResetController()
        {
            ChartVM.Reset();
            ControllerStarted = false;
            Result = "Résultat en attente";
        }

        private bool CanStartController()
        {
            return !ControllerStarted & VerifyDate();
        }

        private bool CanStopController()
        {
            return ControllerStarted;
        }


        private bool VerifyDate()
        {
            DateTime firstDate = new DateTime(2009, 12, 14);
            DateTime lastDate = new DateTime(2013, 06, 13);
            DateTime firstDateHistoric = new DateTime(2010, 01, 01);
            DateTime lastDateHistoric = new DateTime(2015, 08, 20);
            List<System.DayOfWeek> weekEndDays = new List<System.DayOfWeek>() { DayOfWeek.Sunday, DayOfWeek.Saturday };
            if (!(!weekEndDays.Contains(StartDate.DayOfWeek) & !weekEndDays.Contains(EndDate.DayOfWeek) & ((StartDate >= firstDate & EndDate <= lastDate & configVM.dataType == "SemiHistoricData") | (StartDate >= firstDateHistoric & EndDate <= lastDateHistoric & configVM.dataType == "HistoricData") | configVM.dataType == "SimulatedData") & StartDate < EndDate))
            {
                ErrorMessage = "Erreur: Dates choisies invalides";
                return false;
            }
            else if (DayCount.CountBusinessDays(StartDate, EndDate) < WindowSizeVM)
            {
                ErrorMessage = "Erreur: Problème sur la taille de la fenêtre d'estimation";
                return false;
            }
            else if (DayCount.CountBusinessDays(StartDate, EndDate) < RebalancingRateVM)
            {
                ErrorMessage = "Erreur: Problème sur la taille de l'intervalle de rebalancement";
                return false;
            }
            else if (ControllerStarted)
            {
                return true;
            }
            else
            {
                ErrorMessage = "";
                return true;
            }
        }

        private void configInfos()
        {
            string res = "Infos sur l'option :\n";
            res += "Strike : " + configVM.strike + "\n";
            res += "Maturity : " + configVM.maturity + "\n";
            res += "Actions sous-jacentes : ";
            var i = 0;
            foreach(Share share in configVM.underlyingShares)
            {
                res += share.Name + " (" + configVM.weights[i] + ") ";
            }
            res += "\nData type : " + dataVM.Name + "\n";
            res += "Is estimated : " + configVM.isEstimated + "\n";
            res += "Window size : " + configVM.estimatedWindowSize + "\n";
            res += "Rebalancing rate : " + configVM.rebalancingRate;
            Infos = res;
        }
    }
}
