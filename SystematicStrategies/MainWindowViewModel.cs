using System;
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

namespace SystematicStrategies
{
    internal class MainWindowViewModel : BindableBase
    {
        private Controller controller;
        private bool controllerStarted;
        private string _result = "Résultat en attente";
        public MainWindowViewModel()
        {
            //FirstDate = new DatePicker();
            //FirstDate.SelectedDate = new DateTime(2009, 12, 12);
            FirstDate = new DateTime(2009, 12, 12);
            LastDate = new DateTime(2010, 10, 30);
            StartCommand = new DelegateCommand(StartController, CanStartController);
            ResetCommand = new DelegateCommand(ResetController, CanStopController);

        }

        public DateTime FirstDate { get; set; }

        public DateTime LastDate { get; set; }

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
            Share action = new Share("AC FP", "AC FP");
            SemiHistoricDataFeedProvider sdf = new SemiHistoricDataFeedProvider();
            var strike = 12;
            VanillaCall opt = new VanillaCall("VCall", action, LastDate, strike);
            var strat = new DeltaNeutralStrategy();
            controller = new Controller(opt, FirstDate, LastDate, sdf, strat);
            controller.start();
            ControllerStarted = true;
            Result = controller.ResultToString();
            Result += "\n" + "Date du début : " + FirstDate.ToString();
            Result += "\n" + "Date de fin : " + LastDate.ToString();

        }

        private void ResetController()
        {
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
